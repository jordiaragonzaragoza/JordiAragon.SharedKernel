namespace JordiAragon.SharedKernel.Contracts.Events
{
    /// <summary>
    /// The Event Notification is an event that occurs within the problem (living inside a bounded context)
    /// and is used to communicate a change in the state of the entity or aggregate.
    /// This is a private event, persisted, part of Ubiquitous Language.
    /// </summary>
    /// <typeparam name="TEvent">Source domainEvent.</typeparam>
    public interface IEventNotification<out TEvent> : IEventNotification
        where TEvent : IEvent
    {
        public TEvent Event { get; }
    }
}