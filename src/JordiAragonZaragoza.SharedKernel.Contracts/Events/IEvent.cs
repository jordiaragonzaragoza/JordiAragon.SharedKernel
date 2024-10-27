namespace JordiAragonZaragoza.SharedKernel.Contracts.Events
{
    using System;
    using MediatR;

    /// <summary>
    /// The Event is an event that occurs within the problem (living inside a bounded context)
    /// and is used to communicate a change in the state of the aggregate inside from source transaction.
    /// This is a private event, not persisted, part of Ubiquitous Language.
    /// </summary>
    public interface IEvent : INotification
    {
        public Guid Id { get; }

        public bool IsPublished { get; set; }

        public DateTimeOffset DateOccurredOnUtc { get; }
    }
}