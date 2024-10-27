namespace JordiAragonZaragoza.SharedKernel.Application.Contracts.Interfaces
{
    using JordiAragonZaragoza.SharedKernel.Contracts.Events;
    using MediatR;

    public interface IEventHandler<in TEvent> : INotificationHandler<TEvent>
        where TEvent : IEvent
    {
    }
}