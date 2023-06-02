namespace JordiAragon.SharedKernel.Application.Contracts.Interfaces
{
    using MediatR;

    /// <summary>
    /// Handle the application event in the same source transaction.
    /// </summary>
    /// <typeparam name="TApplicationEvent">The source application event.</typeparam>
    public interface IApplicationEventHandler<in TApplicationEvent> : INotificationHandler<TApplicationEvent>
        where TApplicationEvent : IApplicationEvent
    {
    }
}