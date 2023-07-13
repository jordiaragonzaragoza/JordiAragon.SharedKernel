namespace JordiAragon.SharedKernel.Contracts.Events
{
    using System.Collections.Generic;

    public interface IEventsContainer<out TEvent>
        where TEvent : IEvent
    {
        IEnumerable<TEvent> Events { get; }

        void ClearEvents();
    }
}