namespace JordiAragon.SharedKernel.Infrastructure.Outbox
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using System.Text.Json;
    using System.Threading;
    using System.Threading.Tasks;
    using JordiAragon.SharedKernel.Contracts.Events;
    using JordiAragon.SharedKernel.Contracts.Repositories;
    using JordiAragon.SharedKernel.Domain.Contracts.Interfaces;
    using MediatR;
    using Microsoft.Extensions.Logging;
    using Polly;
    using Polly.Retry;
    using Quartz;

    [DisallowConcurrentExecution]
    public abstract class ProcessOutboxMessagesJob : IJob
    {
        private readonly IDateTime dateTime;
        private readonly IPublisher internalBus;
        private readonly ILogger<ProcessOutboxMessagesJob> logger;
        private readonly ICachedSpecificationRepository<OutboxMessage, Guid> repositoryOutboxMessages;

        protected ProcessOutboxMessagesJob(
            IDateTime dateTime,
            IPublisher internalBus,
            ILogger<ProcessOutboxMessagesJob> logger,
            ICachedSpecificationRepository<OutboxMessage, Guid> repositoryOutboxMessages)
        {
            this.dateTime = dateTime ?? throw new ArgumentNullException(nameof(dateTime));
            this.internalBus = internalBus ?? throw new ArgumentNullException(nameof(internalBus));
            this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
            this.repositoryOutboxMessages = repositoryOutboxMessages ?? throw new ArgumentNullException(nameof(repositoryOutboxMessages));
        }

        protected abstract IEnumerable<Assembly> CurrentAssemblies { get; }

        public async Task Execute(IJobExecutionContext context)
        {
            var outboxMessages = await this.repositoryOutboxMessages.ListAsync(new OutboxMessagesUnProcessedWithoutErrorSpec(), context.CancellationToken);
            if (!outboxMessages.Any())
            {
                return;
            }

            foreach (var outboxMessage in outboxMessages)
            {
                Type type = this.CurrentAssemblies
                                .Select(a => a.GetType(outboxMessage.Type))
                                .FirstOrDefault(t => t != null);

                object messageDeserialized;
                try
                {
                    messageDeserialized = JsonSerializer.Deserialize(outboxMessage.Content, type);
                }
                catch (Exception exception)
                {
                    this.logger.LogError(
                       exception,
                       "Error Deserializing Outbox Message. Id: {@Id}  Type: {Type} Content: {Content}.",
                       outboxMessage.Id,
                       outboxMessage.Type,
                       outboxMessage.Content);

                    continue;
                }

                if (messageDeserialized is null)
                {
                    this.logger.LogError(
                       "Error Deserializing Outbox Message. Id: {@Id}  Type: {Type} Content: {Content}.",
                       outboxMessage.Id,
                       outboxMessage.Type,
                       outboxMessage.Content);

                    continue;
                }

                if (messageDeserialized is IEventNotification eventNotification)
                {
                    await this.PublishAsync(outboxMessage, eventNotification, context.CancellationToken);
                }
            }

            await this.repositoryOutboxMessages.UpdateRangeAsync(outboxMessages, context.CancellationToken);
        }

        private async Task PublishAsync(OutboxMessage outboxMessage, IEventNotification eventNotification, CancellationToken cancellationToken = default)
        {
            try
            {
                this.logger.LogInformation("Dispatched: Event notification {EventNofification}", eventNotification.GetType().Name);

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

                await pipeline.ExecuteAsync(async token => await this.internalBus.Publish(eventNotification, token), cancellationToken);

                outboxMessage.DateProcessedOnUtc = this.dateTime.UtcNow;
            }
            catch (Exception exception)
            {
                this.logger.LogError(
                   exception,
                   "Error publishing EventNotification: {@Name} {Content}.",
                   eventNotification.GetType().Name,
                   eventNotification);

                outboxMessage.Error = $"Message: {exception.Message}\nStackTrace: {exception.StackTrace}";
            }
        }
    }
}