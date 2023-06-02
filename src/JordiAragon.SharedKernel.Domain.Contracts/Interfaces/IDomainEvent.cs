namespace JordiAragon.SharedKernel.Domain.Contracts.Interfaces
{
    using JordiAragon.SharedKernel.Contracts.Events;

    /// <summary>
    /// The Domain Event is an event that occurs within the problem domain (living inside a bounded context)
    /// and is used to communicate a change in the state of the entity.
    /// This is a private event, not persisted, part of Ubiquitous Language.
    /// </summary>
    public interface IDomainEvent : IEvent
    {
    }
}