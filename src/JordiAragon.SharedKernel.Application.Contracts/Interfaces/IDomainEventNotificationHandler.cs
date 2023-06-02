namespace JordiAragon.SharedKernel.Application.Contracts.Interfaces
{
    using JordiAragon.SharedKernel.Domain.Contracts.Interfaces;
    using MediatR;

    /// <summary>
    /// Handle the domain event out side the source transaction.
    /// </summary>
    /// <typeparam name="TDomainEventNotification">The domain event notification.</typeparam>
    public interface IDomainEventNotificationHandler<in TDomainEventNotification> : INotificationHandler<TDomainEventNotification>
        where TDomainEventNotification : IDomainEventNotification
    {
    }
}