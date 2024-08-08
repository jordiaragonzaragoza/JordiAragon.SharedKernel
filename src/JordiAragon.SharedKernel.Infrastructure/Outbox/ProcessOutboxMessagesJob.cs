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
    using Microsoft.Extensions.Logging;
    using Quartz;

    [DisallowConcurrentExecution]
    public abstract class ProcessOutboxMessagesJob : IJob
    {
        private readonly IDateTime dateTime;
        private readonly IEventBus eventBus;
        private readonly ILogger<ProcessOutboxMessagesJob> logger;
        private readonly ICachedSpecificationRepository<OutboxMessage, Guid> repositoryOutboxMessages;

        protected ProcessOutboxMessagesJob(
            IDateTime dateTime,
            IEventBus eventBus,
            ILogger<ProcessOutboxMessagesJob> logger,
            ICachedSpecificationRepository<OutboxMessage, Guid> repositoryOutboxMessages)
        {
            this.dateTime = dateTime ?? throw new ArgumentNullException(nameof(dateTime));
            this.eventBus = eventBus ?? throw new ArgumentNullException(nameof(eventBus));
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
                Type? type = this.CurrentAssemblies
                                .Select(assembly => assembly.GetType(outboxMessage.Type))
                                .FirstOrDefault(type => type != null);

                if (type is null)
                {
                    this.logger.LogError(
                        "Error finding type for Outbox Message. Id: {@Id}  Type: {Type} Content:{Content}",
                        outboxMessage.Id,
                        outboxMessage.Type,
                        outboxMessage.Content);

                    continue;
                }

                object? messageDeserialized;
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
                await this.eventBus.PublishAsync(eventNotification, cancellationToken);

                outboxMessage.DateProcessedOnUtc = this.dateTime.UtcNow;
            }
            catch (Exception exception)
            {
                outboxMessage.Error = $"Message: {exception.Message}\nStackTrace: {exception.StackTrace}";
            }
        }
    }
}