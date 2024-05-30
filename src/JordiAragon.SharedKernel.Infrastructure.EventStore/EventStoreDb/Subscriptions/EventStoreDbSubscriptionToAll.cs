namespace JordiAragon.SharedKernel.Infrastructure.EventStore.EventStoreDb.Subscriptions
{
    using System;
    using System.Collections.Generic;
    using System.Threading;
    using System.Threading.Tasks;
    using Ardalis.GuardClauses;
    using Autofac;
    using Autofac.Core;
    using global::EventStore.Client;
    using Grpc.Core;
    using JordiAragon.SharedKernel.Contracts.DependencyInjection;
    using JordiAragon.SharedKernel.Contracts.Events;
    using JordiAragon.SharedKernel.Contracts.Repositories;
    using JordiAragon.SharedKernel.Domain.Contracts.Interfaces;
    using JordiAragon.SharedKernel.Helpers;
    using JordiAragon.SharedKernel.Infrastructure.EventStore.EventStoreDb.Serialization;
    using JordiAragon.SharedKernel.Infrastructure.ProjectionCheckpoint;
    using MediatR;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Logging;

    public class EventStoreDbSubscriptionToAll : ITransientDependency
    {
        private readonly EventStoreClient eventStoreClient;
        private readonly EventTypeMapper eventTypeMapper;
        private readonly ILogger<EventStoreDbSubscriptionToAll> logger;
        private readonly IDateTime datetime;
        private readonly object resubscribeLock = new();
        private readonly ILifetimeScope lifetimeScope;

        private IServiceScopeFactory serviceScopeFactory;
        private EventStoreDbSubscriptionToAllOptions subscriptionOptions;
        private CancellationToken cancellationToken;

        public EventStoreDbSubscriptionToAll(
            ILifetimeScope lifetimeScope,
            EventStoreClient eventStoreClient,
            EventTypeMapper eventTypeMapper,
            ILogger<EventStoreDbSubscriptionToAll> logger,
            IDateTime datetime)
        {
            this.lifetimeScope = Guard.Against.Null(lifetimeScope, nameof(lifetimeScope));
            this.eventStoreClient = Guard.Against.Null(eventStoreClient, nameof(eventStoreClient));
            this.eventTypeMapper = Guard.Against.Null(eventTypeMapper, nameof(eventTypeMapper));
            this.logger = Guard.Against.Null(logger, nameof(logger));
            this.datetime = Guard.Against.Null(datetime, nameof(datetime));
        }

        private Guid SubscriptionId => this.subscriptionOptions.SubscriptionId;

        public async Task SubscribeToAllAsync(IServiceScopeFactory serviceScopeFactory, EventStoreDbSubscriptionToAllOptions subscriptionOptions, CancellationToken cancellationToken)
        {
            // see: https://github.com/dotnet/runtime/issues/36063
            await Task.Yield();

            this.serviceScopeFactory = serviceScopeFactory;
            this.subscriptionOptions = subscriptionOptions;
            this.cancellationToken = cancellationToken;

            this.logger.LogInformation("Subscription to all '{SubscriptionId}'", subscriptionOptions.SubscriptionId);

            // Required to get scoped services on a background service.
            using var scope = this.serviceScopeFactory.CreateScope();
            var checkpointRepository = scope.ServiceProvider.GetRequiredService<IRepository<Checkpoint, Guid>>();

            var checkpoint = await checkpointRepository.GetByIdAsync(this.SubscriptionId, cancellationToken).ConfigureAwait(false);

            await this.eventStoreClient.SubscribeToAllAsync(
                checkpoint == null ? FromAll.Start : FromAll.After(new Position(checkpoint.Position, checkpoint.Position)),
                this.HandleEventAsync,
                subscriptionOptions.ResolveLinkTos,
                this.HandleDrop,
                subscriptionOptions.FilterOptions,
                subscriptionOptions.Credentials,
                cancellationToken).ConfigureAwait(false);

            this.logger.LogInformation("Subscription to all '{SubscriptionId}' started", this.SubscriptionId);
        }

        private async Task HandleEventAsync(StreamSubscription subscription, ResolvedEvent resolvedEvent, CancellationToken cancellationToken)
        {
            try
            {
                if (this.IsEventWithEmptyData(resolvedEvent) || this.IsCheckpointEvent(resolvedEvent))
                {
                    return;
                }

                var domainEvent = SerializerHelper.Deserialize(resolvedEvent);
                var eventNotification = this.CreateEventNotification(domainEvent);

                // Required to get scoped services on a background service.
                using var scope = this.serviceScopeFactory.CreateScope();

                var internalBus = scope.ServiceProvider.GetRequiredService<IPublisher>();
                var checkpointRepository = scope.ServiceProvider.GetRequiredService<IRepository<Checkpoint, Guid>>();

                // publish event to internal event bus
                await internalBus.Publish(eventNotification, cancellationToken);

                var existingCheckpoint = await checkpointRepository.GetByIdAsync(this.SubscriptionId, cancellationToken);
                if (existingCheckpoint is not null)
                {
                    existingCheckpoint.Position = resolvedEvent.Event.Position.CommitPosition;
                    existingCheckpoint.CheckpointedAtOnUtc = this.datetime.UtcNow;

                    await checkpointRepository.UpdateAsync(existingCheckpoint, cancellationToken)
                    .ConfigureAwait(false);

                    this.logger.LogInformation("Added checkpoint: {Checkpoint}", existingCheckpoint);

                    return;
                }

                var checkpoint = new Checkpoint(this.SubscriptionId, resolvedEvent.Event.Position.CommitPosition, this.datetime.UtcNow);
                await checkpointRepository.AddAsync(checkpoint, cancellationToken)
                    .ConfigureAwait(false);

                this.logger.LogInformation("Added checkpoint: {Checkpoint}", checkpoint);
            }
            catch (Exception e)
            {
                this.logger.LogError("Error consuming message: {ExceptionMessage}{ExceptionStackTrace}", e.Message, e.StackTrace);

                // if you're fine with dropping some events instead of stopping subscription
                // then you can add some logic if error should be ignored
                throw;
            }
        }

        private void HandleDrop(StreamSubscription subscription, SubscriptionDroppedReason reason, Exception exception)
        {
            if (exception is RpcException { StatusCode: StatusCode.Cancelled })
            {
                this.logger.LogWarning(
                    "Subscription to all '{SubscriptionId}' dropped by client",
                    this.SubscriptionId);

                return;
            }

            this.logger.LogError(
                exception,
                "Subscription to all '{SubscriptionId}' dropped with '{StatusCode}' and '{Reason}'",
                this.SubscriptionId,
                (exception as RpcException)?.StatusCode ?? StatusCode.Unknown,
                reason);

            this.Resubscribe();
        }

        private void Resubscribe()
        {
            // You may consider adding a max resubscribe count if you want to fail process
            // instead of retrying until database is up
            while (true)
            {
                var resubscribed = false;
                try
                {
                    Monitor.Enter(this.resubscribeLock);

                    // No synchronization context is needed to disable synchronization context.
                    // That enables running asynchronous method not causing deadlocks.
                    // As this is a background process then we don't need to have async context here.
                    using (NoSynchronizationContextScopeHelper.Enter())
                    {
#pragma warning disable VSTHRD002
                        this.SubscribeToAllAsync(this.serviceScopeFactory, this.subscriptionOptions, this.cancellationToken).Wait(this.cancellationToken);
#pragma warning restore VSTHRD002
                    }

                    resubscribed = true;
                }
                catch (Exception exception)
                {
                    this.logger.LogWarning(
                        exception,
                        "Failed to resubscribe to all '{SubscriptionId}' dropped with '{ExceptionMessage}{ExceptionStackTrace}'",
                        this.SubscriptionId,
                        exception.Message,
                        exception.StackTrace);
                }
                finally
                {
                    Monitor.Exit(this.resubscribeLock);
                }

                if (resubscribed)
                {
                    break;
                }

                // Sleep between reconnections to not flood the database or not kill the CPU with infinite loop
                // Randomness added to reduce the chance of multiple subscriptions trying to reconnect at the same time
                Thread.Sleep(1000 + new Random((int)DateTime.UtcNow.Ticks).Next(1000));
            }
        }

        private bool IsEventWithEmptyData(ResolvedEvent resolvedEvent)
        {
            if (resolvedEvent.Event.Data.Length != 0)
            {
                return false;
            }

            this.logger.LogInformation("Event without data received");

            return true;
        }

        private bool IsCheckpointEvent(ResolvedEvent resolvedEvent)
        {
            if (resolvedEvent.Event.EventType != this.eventTypeMapper.ToName<Checkpoint>())
            {
                return false;
            }

            this.logger.LogInformation("Checkpoint event - ignoring");

            return true;
        }

        private IEventNotification<IEvent> CreateEventNotification(IEvent @event)
        {
            // Instanciate the event notification.
            Type eventNotificationType = typeof(IEventNotification<>);
            var notificationWithGenericType = eventNotificationType.MakeGenericType(@event.GetType());
            var notification = this.lifetimeScope.ResolveOptional(notificationWithGenericType, new List<Parameter>
                {
                    new NamedParameter("Event", @event),
                });

            return notification as IEventNotification<IEvent>;
        }
    }
}