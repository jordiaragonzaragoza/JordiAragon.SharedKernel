namespace JordiAragon.SharedKernel.Application.Contracts.Interfaces
{
    using JordiAragon.SharedKernel.Contracts.Events;

    /// <summary>
    /// The Application Event Notification is an event that occurs within the problem domain (living inside a bounded context)
    /// and is used to communicate a change in the state of the entity or aggregate.
    /// This is a private event, persisted, part of Ubiquitous Language.
    /// </summary>
    /// <typeparam name="TApplicationEvent">Source applicationEvent.</typeparam>
    public interface IApplicationEventNotification<out TApplicationEvent> : IEventNotification<TApplicationEvent>, IApplicationEventNotification
        where TApplicationEvent : IApplicationEvent
    {
    }
}