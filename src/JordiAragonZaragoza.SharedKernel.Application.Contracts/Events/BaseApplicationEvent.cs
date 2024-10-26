namespace JordiAragonZaragoza.SharedKernel.Application.Contracts.Events
{
    using System;
    using JordiAragonZaragoza.SharedKernel.Application.Contracts.Interfaces;

    public abstract record class BaseApplicationEvent : IApplicationEvent
    {
        public Guid Id { get; protected init; } = Guid.NewGuid();

        public bool IsPublished { get; set; }

        public DateTimeOffset DateOccurredOnUtc { get; protected init; } = DateTimeOffset.UtcNow;
    }
}