namespace JordiAragon.SharedKernel.Application.Contracts.Interfaces
{
    using MediatR;

    /// <summary>
    /// Handle the application event out side the source transaction.
    /// </summary>
    /// <typeparam name="TApplicationEventNotification">The application event notification.</typeparam>
    public interface IApplicationEventNotificationHandler<in TApplicationEventNotification> : INotificationHandler<TApplicationEventNotification>
        where TApplicationEventNotification : IApplicationEventNotification
    {
    }
}