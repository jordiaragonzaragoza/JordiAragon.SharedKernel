namespace JordiAragon.SharedKernel.Domain.Events.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;
    using Ardalis.GuardClauses;
    using Autofac;
    using Autofac.Core;
    using JordiAragon.SharedKernel.Contracts.Events;
    using JordiAragon.SharedKernel.Contracts.Outbox;
    using MediatR;
    using Microsoft.Extensions.Logging;

    public class EventsDispatcherService : IEventsDispatcherService
    {
        private readonly IPublisher publisher;
        private readonly ILifetimeScope scope;
        private readonly IOutboxService outboxService;
        private readonly ILogger<EventsDispatcherService> logger;

        public EventsDispatcherService(
            IPublisher publisher,
            ILifetimeScope scope,
            IOutboxService outboxService,
            ILogger<EventsDispatcherService> logger)
        {
            this.publisher = Guard.Against.Null(publisher, nameof(publisher));
            this.scope = Guard.Against.Null(scope, nameof(scope));
            this.outboxService = Guard.Against.Null(outboxService, nameof(outboxService));
            this.logger = Guard.Against.Null(logger, nameof(logger));
        }

        public async Task DispatchAndClearEventsAsync(IEnumerable<IEventsContainer<IEvent>> eventableEntities, CancellationToken cancellationToken = default)
        {
            var events = eventableEntities.SelectMany(x => x.Events).ToList();

            var eventNotifications = this.CreateEventNotifications(events);

            foreach (var eventsContainer in eventableEntities)
            {
                eventsContainer.ClearEvents();
            }

            await this.PublishEventsAsync(events, cancellationToken);

            await this.StoreEventNotificationsAsync(eventNotifications, cancellationToken);
        }

        private IList<IEventNotification<IEvent>> CreateEventNotifications(IList<IEvent> events)
        {
            var eventNotifications = new List<IEventNotification<IEvent>>();
            foreach (var @event in events)
            {
                // Instanciate the notification.
                Type eventNotificationType = typeof(IEventNotification<>);
                var notificationWithGenericType = eventNotificationType.MakeGenericType(@event.GetType());
                var notification = this.scope.ResolveOptional(notificationWithGenericType, new List<Parameter>
                {
                    new NamedParameter("Event", @event),
                });

                if (notification != null)
                {
                    eventNotifications.Add(notification as IEventNotification<IEvent>);
                }
            }

            return eventNotifications;
        }

        private async Task PublishEventsAsync(IList<IEvent> events, CancellationToken cancellationToken)
        {
            foreach (var @event in events)
            {
                try
                {
                    @event.IsPublished = true;

                    this.logger.LogInformation("Dispatched Event {Event}", @event.GetType().Name);

                    await this.publisher.Publish(@event, cancellationToken).ConfigureAwait(true);
                }
                catch (Exception exception)
                {
                    this.logger.LogError(
                       exception,
                       "Error publishing event: {@Name} {Content}.",
                       @event.GetType().Name,
                       @event);

                    throw;
                }
            }
        }

        private async Task StoreEventNotificationsAsync(IList<IEventNotification<IEvent>> eventNotifications, CancellationToken cancellationToken)
        {
            foreach (var eventNotification in eventNotifications)
            {
                await this.outboxService.AddMessageAsync(eventNotification, cancellationToken);
            }
        }
    }
}