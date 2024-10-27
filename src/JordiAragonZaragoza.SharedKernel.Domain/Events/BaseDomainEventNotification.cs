namespace JordiAragonZaragoza.SharedKernel.Domain.Events
{
    using System;
    using JordiAragonZaragoza.SharedKernel.Contracts.Events;
    using JordiAragonZaragoza.SharedKernel.Domain.Contracts.Interfaces;

    public abstract record class BaseDomainEventNotification<T> : IEventNotification<T>
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