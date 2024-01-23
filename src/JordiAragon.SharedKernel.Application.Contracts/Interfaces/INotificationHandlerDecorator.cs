namespace JordiAragon.SharedKernel.Application.Contracts.Interfaces
{
    using MediatR;

    public interface INotificationHandlerDecorator<in TNotification> : INotificationHandler<TNotification>
        where TNotification : INotification
    {
        INotificationHandler<TNotification> DecoratedHandler { get; }
    }
}