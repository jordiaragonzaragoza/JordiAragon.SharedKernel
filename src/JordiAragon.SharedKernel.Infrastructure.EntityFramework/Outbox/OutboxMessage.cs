namespace JordiAragon.SharedKernel.Infrastructure.EntityFramework.Outbox
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations.Schema;
    using JordiAragon.SharedKernel.Domain.Contracts.Interfaces;
    using JordiAragon.SharedKernel.Domain.Entities;

    public class OutboxMessage : BaseEntity<OutboxMessageId>, IAggregateRoot<OutboxMessageId>
    {
        private readonly List<IDomainEvent> domainEvents = new();

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

        [NotMapped]
        public IEnumerable<IDomainEvent> Events => this.domainEvents.AsReadOnly();

        public static OutboxMessage Create(
            Guid id,
            DateTime dateOccurredOnUtc,
            string type,
            string content)
        {
            return new OutboxMessage(OutboxMessageId.Create(id), dateOccurredOnUtc, type, content);
        }

        public void ClearEvents() => this.domainEvents.Clear();
    }
}