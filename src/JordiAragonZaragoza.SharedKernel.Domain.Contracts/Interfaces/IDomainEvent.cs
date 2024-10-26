namespace JordiAragonZaragoza.SharedKernel.Domain.Contracts.Interfaces
{
    using System;
    using JordiAragonZaragoza.SharedKernel.Contracts.Events;

    /// <summary>
    /// The Event occurs within the problem domain (living inside a bounded context)
    /// and is used to communicate a change in the state of the entity.
    /// This is a private event part of Ubiquitous Language.
    /// </summary>
    public interface IDomainEvent : IEvent
    {
        public Guid AggregateId { get; }
    }
}