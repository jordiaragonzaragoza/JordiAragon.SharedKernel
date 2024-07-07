namespace JordiAragon.SharedKernel.Application.Contracts.Interfaces
{
    using JordiAragon.SharedKernel.Contracts.Events;
    using MediatR;

    public interface IEventHandler<in TEvent> : INotificationHandler<TEvent>
        where TEvent : IEvent
    {
    }
}