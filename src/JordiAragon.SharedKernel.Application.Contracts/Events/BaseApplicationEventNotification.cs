namespace JordiAragon.SharedKernel.Application.Contracts.Events
{
    using System;
    using System.Text.Json.Serialization;
    using JordiAragon.SharedKernel.Application.Contracts.Interfaces;

    public abstract record class BaseApplicationEventNotification<T> : IApplicationEventNotification<T>
        where T : IApplicationEvent
    {
        protected BaseApplicationEventNotification()
        {
            this.Id = Guid.NewGuid();
        }

        protected BaseApplicationEventNotification(T applicationEvent)
        {
            this.Id = Guid.NewGuid();
            this.Event = applicationEvent;
        }

        [JsonIgnore]
        public T Event { get; init; }

        public Guid Id { get; init; }
    }
}