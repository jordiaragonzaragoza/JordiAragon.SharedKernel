namespace JordiAragon.SharedKernel.Infrastructure.EntityFramework.Outbox
{
    using System;
    using JordiAragon.SharedKernel.Domain.Entities;
    using JordiAragon.SharedKernel.Domain.Contracts.Interfaces;

    public class OutboxMessage : BaseEntity<OutboxMessageId>, IAggregateRoot
    {
        private OutboxMessage(
            OutboxMessageId id,
            DateTime dateOccurredOnUtc,
            string type,
            string content)
            : base(id)
        {
            this.DateOccurredOnUtc = dateOccurredOnUtc;
            this.Type = type;
            this.Content = content;
        }

        public DateTime DateOccurredOnUtc { get; private init; }

        public string Type { get; private init; }

        public string Content { get; private init; }

        public DateTime? DateProcessedOnUtc { get; set; }

        public string Error { get; set; }

        public static OutboxMessage Create(
            Guid id,
            DateTime dateOccurredOnUtc,
            string type,
            string content)
        {
            return new OutboxMessage(OutboxMessageId.Create(id), dateOccurredOnUtc, type, content);
        }
    }
}