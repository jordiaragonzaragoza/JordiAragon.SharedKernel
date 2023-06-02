namespace JordiAragon.SharedKernel.Application.Contracts.Interfaces
{
    using JordiAragon.SharedKernel.Domain.Contracts.Interfaces;
    using MediatR;

    /// <summary>
    /// Handle the domain event in the same source transaction.
    /// </summary>
    /// <typeparam name="TDomainEvent">The source domain event.</typeparam>
    public interface IDomainEventHandler<in TDomainEvent> : INotificationHandler<TDomainEvent>
        where TDomainEvent : IDomainEvent
    {
    }
}