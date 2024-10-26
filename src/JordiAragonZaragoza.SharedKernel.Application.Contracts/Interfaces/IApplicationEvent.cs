namespace JordiAragonZaragoza.SharedKernel.Application.Contracts.Interfaces
{
    using JordiAragonZaragoza.SharedKernel.Contracts.Events;

    /// <summary>
    /// The Application Event is an event that occurs within the problem domain (living inside a bounded context)
    /// and is used to communicate a change in the state of the aggregate.
    /// This is a private event, not persisted, part of Ubiquitous Language.
    /// </summary>
    public interface IApplicationEvent : IEvent
    {
    }
}