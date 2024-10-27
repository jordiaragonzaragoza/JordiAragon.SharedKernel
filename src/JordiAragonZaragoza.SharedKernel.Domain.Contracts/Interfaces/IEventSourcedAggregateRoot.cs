namespace JordiAragonZaragoza.SharedKernel.Domain.Contracts.Interfaces
{
    using System.Collections.Generic;

    // Apply this marker interface only to event sourced aggregate root entities
    // Write repositories will only work with aggregate roots.
    public interface IEventSourcedAggregateRoot<out TId> : IAggregateRoot<TId>
         where TId : class, IEntityId
    {
        void Load(IEnumerable<IDomainEvent> history);
    }
}