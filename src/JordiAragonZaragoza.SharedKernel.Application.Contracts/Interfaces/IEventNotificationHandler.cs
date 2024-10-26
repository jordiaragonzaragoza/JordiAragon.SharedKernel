namespace JordiAragonZaragoza.SharedKernel.Application.Contracts.Interfaces
{
    using JordiAragonZaragoza.SharedKernel.Contracts.Events;
    using MediatR;

    public interface IEventNotificationHandler<in TEventNotification> : INotificationHandler<TEventNotification>
        where TEventNotification : IEventNotification
    {
    }
}