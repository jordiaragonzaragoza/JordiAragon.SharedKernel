namespace JordiAragon.SharedKernel.Infrastructure.EntityFramework.Outbox
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using System.Text.Json;
    using System.Threading;
    using System.Threading.Tasks;
    using JordiAragon.SharedKernel.Contracts.Events;
    using JordiAragon.SharedKernel.Domain.Contracts.Interfaces;
    using MediatR;
    using Microsoft.Extensions.Logging;
    using Quartz;

    [DisallowConcurrentExecution]
    public abstract class ProcessOutboxMessagesJob : IJob
    {
        private readonly IDateTime dateTime;
        private readonly IPublisher mediator;
        private readonly ILogger<ProcessOutboxMessagesJob> logger;
        private readonly ICachedRepository<OutboxMessage, OutboxMessageId, Guid> repositoryOutboxMessages;

        protected ProcessOutboxMessagesJob(
            IDateTime dateTime,
            IPublisher mediator,
            ILogger<ProcessOutboxMessagesJob> logger,
            ICachedRepository<OutboxMessage, OutboxMessageId, Guid> repositoryOutboxMessages)
        {
            this.dateTime = dateTime ?? throw new ArgumentNullException(nameof(dateTime));
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
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

                await this.mediator.Publish(eventNotification, cancellationToken);

                outboxMessage.DateProcessedOnUtc = this.dateTime.UtcNow;
            }
            catch (Exception exception)
            {
                this.logger.LogError(
                   exception,
                   "Error publishing EventNotification: {@Name} {Content}.",
                   eventNotification.GetType().Name,
                   eventNotification);

                outboxMessage.Error = exception.Message;
            }
        }
    }
}