namespace JordiAragon.SharedKernel.Infrastructure.Outbox
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;
    using JordiAragon.SharedKernel.Application.Contracts.Interfaces;
    using JordiAragon.SharedKernel.Contracts.Events;
    using JordiAragon.SharedKernel.Contracts.Outbox;
    using JordiAragon.SharedKernel.Contracts.Repositories;
    using Microsoft.Extensions.Logging;
    using Newtonsoft.Json;

    public class OutboxService : IOutboxService
    {
        private readonly IIdGenerator guidGenerator;
        private readonly ICachedSpecificationRepository<OutboxMessage, Guid> repositoryOutboxMessage;
        private readonly ILogger<OutboxService> logger;

        public OutboxService(
            IIdGenerator guidGenerator,
            ICachedSpecificationRepository<OutboxMessage, Guid> repositoryOutboxMessage,
            ILogger<OutboxService> logger)
        {
            this.guidGenerator = guidGenerator ?? throw new ArgumentNullException(nameof(guidGenerator));
            this.repositoryOutboxMessage = repositoryOutboxMessage ?? throw new ArgumentNullException(nameof(repositoryOutboxMessage));
            this.logger = logger;
        }

        public async Task AddMessageAsync(IEventNotification<IEvent> eventNotification, CancellationToken cancellationToken = default)
        {
            var type = eventNotification.GetType().FullName;
            var data = JsonConvert.SerializeObject(eventNotification); // TODO: Change to System.Text.Json when nested serialization is supported.

            var outboxMessage = new OutboxMessage(
                this.guidGenerator.Create(),
                eventNotification.Event.DateOccurredOnUtc,
                type,
                data);

            await this.repositoryOutboxMessage.AddAsync(outboxMessage, cancellationToken);

            this.logger.LogInformation("Stored Notification Event: {Event}", eventNotification.GetType().Name);
        }
    }
}