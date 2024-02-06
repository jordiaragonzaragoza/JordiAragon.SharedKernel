namespace JordiAragon.SharedKernel.Infrastructure.EntityFramework.Outbox
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;
    using JordiAragon.SharedKernel.Contracts.Events;
    using JordiAragon.SharedKernel.Contracts.Outbox;
    using JordiAragon.SharedKernel.Contracts.Repositories;
    using Microsoft.Extensions.Logging;
    using Newtonsoft.Json;
    using Volo.Abp.Guids;

    public class OutboxService : IOutboxService
    {
        private readonly IGuidGenerator guidGenerator;
        private readonly ICachedSpecificationRepository<OutboxMessage, OutboxMessageId> repositoryOutboxMessage;
        private readonly ILogger<OutboxService> logger;

        public OutboxService(
            IGuidGenerator guidGenerator,
            ICachedSpecificationRepository<OutboxMessage, OutboxMessageId> repositoryOutboxMessage,
            ILogger<OutboxService> logger)
        {
            this.guidGenerator = guidGenerator ?? throw new ArgumentNullException(nameof(guidGenerator));
            this.repositoryOutboxMessage = repositoryOutboxMessage ?? throw new ArgumentNullException(nameof(repositoryOutboxMessage));
            this.logger = logger;
        }

        public async Task AddMessageAsync(IEventNotification<IEvent> eventNotification, CancellationToken cancellationToken = default)
        {
            var type = eventNotification.GetType().FullName;
            var data = JsonConvert.SerializeObject(eventNotification); // TODO: Change to System.Text.Json

            var outboxMessage = OutboxMessage.Create(
                id: this.guidGenerator.Create(),
                dateOccurredOnUtc: eventNotification.Event.DateOccurredOnUtc,
                type: type,
                content: data);

            await this.repositoryOutboxMessage.AddAsync(outboxMessage, cancellationToken);

            this.logger.LogInformation("Stored Notification Event: {Event}", eventNotification.GetType().Name);
        }
    }
}