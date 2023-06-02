namespace JordiAragon.SharedKernel.Application.Contracts.Events
{
    using System;
    using JordiAragon.SharedKernel.Application.Contracts.Interfaces;

    public abstract record class BaseApplicationEvent : IApplicationEvent
    {
        public Guid Id { get; protected init; } = Guid.NewGuid();

        public bool IsPublished { get; set; }

        public DateTime DateOccurredOnUtc { get; protected init; } = DateTime.UtcNow;
    }
}