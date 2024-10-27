namespace JordiAragonZaragoza.SharedKernel.Domain.Contracts.Interfaces
{
    using JordiAragonZaragoza.SharedKernel.Contracts.Events;
    using JordiAragonZaragoza.SharedKernel.Contracts.Model;

    // Apply this marker interface only to aggregate root entities
    // Write repositories will only work with aggregate roots.
    public interface IAggregateRoot<out TId> : IEntity<TId>, IEventsContainer<IDomainEvent>, IOptimisticLockable
        where TId : class, IEntityId
    {
    }
}