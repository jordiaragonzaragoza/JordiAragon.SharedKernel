namespace JordiAragonZaragoza.SharedKernel.Infrastructure.InternalBus
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;
    using Ardalis.GuardClauses;
    using global::MediatR;
    using JordiAragonZaragoza.SharedKernel.Contracts.Events;
    using JordiAragonZaragoza.SharedKernel.Domain.Contracts.Interfaces;
    using Microsoft.Extensions.Logging;
    using Polly;
    using Polly.Retry;

    public class EventBus : IEventBus
    {
        private readonly IPublisher publisher;
        private readonly ILogger<EventBus> logger;

        public EventBus(
            IPublisher publisher,
            ILogger<EventBus> logger)
        {
            this.publisher = Guard.Against.Null(publisher, nameof(publisher));
            this.logger = Guard.Against.Null(logger, nameof(logger));
        }

        public async Task PublishAsync(IEvent @event, CancellationToken cancellationToken = default)
        {
            ArgumentNullException.ThrowIfNull(@event, nameof(@event));

            try
            {
                @event.IsPublished = true;

                this.logger.LogInformation("Dispatched Event {Event}", @event.GetType().Name);

                await this.publisher.Publish(@event, cancellationToken).ConfigureAwait(true);
            }
            catch (Exception exception)
            {
                @event.IsPublished = false;

                this.logger.LogError(
                   exception,
                   "Error publishing event: {@Name} {Content}.",
                   @event.GetType().Name,
                   @event);

                throw;
            }
        }

        public async Task PublishAsync(IEventNotification eventNotification, CancellationToken cancellationToken = default)
        {
            ArgumentNullException.ThrowIfNull(eventNotification, nameof(eventNotification));

            try
            {
                this.logger.LogInformation("Dispatched: Event notification {EventNofification}", eventNotification.GetType().Name);

                await this.PublishWithRetryStrategyAsync(eventNotification, cancellationToken);
            }
            catch (Exception exception)
            {
                this.logger.LogError(
                   exception,
                   "Error publishing EventNotification: {@Name} {Content}.",
                   eventNotification.GetType().Name,
                   eventNotification);

                throw;
            }
        }

        /// <summary>
        /// Publish the event using a retry strategy on errors.
        /// Using this method requires using idempotent consumers to avoid duplicate procesing.
        /// </summary>
        /// <param name="eventNotification">The event.</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        /// <returns>An awaitable task.</returns>
        private async Task PublishWithRetryStrategyAsync(IEventNotification eventNotification, CancellationToken cancellationToken)
        {
            var pipeline = new ResiliencePipelineBuilder()
                                .AddRetry(new RetryStrategyOptions()
                                {
                                    MaxRetryAttempts = 3,
                                    UseJitter = true,
                                    BackoffType = DelayBackoffType.Exponential,
                                    Delay = TimeSpan.FromMilliseconds(500),
                                    OnRetry = retryArguments =>
                                    {
                                        this.logger.LogError(
                                           retryArguments.Outcome.Exception,
                                           "Error trying to publish EventNotification: {@Name} Attempt: {AttemptNumber}.",
                                           eventNotification.GetType().Name,
                                           retryArguments.AttemptNumber + 1);

                                        return ValueTask.CompletedTask;
                                    },
                                }).Build();

            await pipeline.ExecuteAsync(async token => await this.publisher.Publish(eventNotification, token), cancellationToken);
        }
    }
}