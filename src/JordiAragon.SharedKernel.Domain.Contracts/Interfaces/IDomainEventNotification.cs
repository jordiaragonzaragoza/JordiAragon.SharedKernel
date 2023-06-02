namespace JordiAragon.SharedKernel.Domain.Contracts.Interfaces
{
    using JordiAragon.SharedKernel.Contracts.Events;

    /// <summary>
    /// The Domain Event Notification is an event that occurs within the problem domain (living inside a bounded context)
    /// and is used to communicate a change in the state of the entity or aggregate.
    /// This is a private event, persisted, part of Ubiquitous Language.
    /// </summary>
    /// <typeparam name="TDomainEvent">Source domainEvent.</typeparam>
    public interface IDomainEventNotification<out TDomainEvent> : IEventNotification<TDomainEvent>, IDomainEventNotification
        where TDomainEvent : IDomainEvent
    {
    }
}