namespace JordiAragon.SharedKernel.Application.Contracts.Interfaces
{
    using JordiAragon.SharedKernel.Contracts.Events;
    using MediatR;

    public interface IEventNotificationHandler<in TEventNotification> : INotificationHandler<TEventNotification>
        where TEventNotification : IEventNotification
    {
    }
}