namespace JordiAragon.SharedKernel.Domain.Contracts.Interfaces
{
    using JordiAragon.SharedKernel.Contracts.Events;

    // Apply this marker interface only to aggregate root entities
    // Write repositories will only work with aggregate roots.
    public interface IAggregateRoot<out TId> : IEntity<TId>, IEventsContainer<IDomainEvent>
    {
    }
}