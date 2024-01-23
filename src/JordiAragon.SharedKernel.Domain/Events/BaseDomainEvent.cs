namespace JordiAragon.SharedKernel.Domain.Events
{
    using System;
    using JordiAragon.SharedKernel.Domain.Contracts.Interfaces;

    public abstract record class BaseDomainEvent(Guid AggregateId) : IDomainEvent
    {
        public Guid Id { get; protected init; } = Guid.NewGuid();

        public bool IsPublished { get; set; }

        public DateTimeOffset DateOccurredOnUtc { get; protected init; } = DateTime.UtcNow;
    }
}