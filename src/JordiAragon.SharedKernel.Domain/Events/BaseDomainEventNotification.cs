namespace JordiAragon.SharedKernel.Domain.Events
{
    using System;
    using JordiAragon.SharedKernel.Domain.Contracts.Interfaces;

    public abstract record class BaseDomainEventNotification<T> : IDomainEventNotification<T>
        where T : IDomainEvent
    {
        protected BaseDomainEventNotification(T domainEvent)
        {
            this.Id = Guid.NewGuid();
            this.Event = domainEvent;
        }

        public T Event { get; init; }

        public Guid Id { get; init; }
    }
}