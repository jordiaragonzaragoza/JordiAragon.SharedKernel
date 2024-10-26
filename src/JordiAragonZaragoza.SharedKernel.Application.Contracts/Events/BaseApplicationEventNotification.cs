namespace JordiAragonZaragoza.SharedKernel.Application.Contracts.Events
{
    using System;
    using JordiAragonZaragoza.SharedKernel.Application.Contracts.Interfaces;
    using JordiAragonZaragoza.SharedKernel.Contracts.Events;

    public abstract record class BaseApplicationEventNotification<T> : IEventNotification<T>
        where T : IApplicationEvent
    {
        protected BaseApplicationEventNotification(T applicationEvent)
        {
            this.Id = Guid.NewGuid();
            this.Event = applicationEvent;
        }

        public T Event { get; init; }

        public Guid Id { get; init; }
    }
}