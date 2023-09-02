namespace JordiAragon.SharedKernel.Domain.Contracts.Interfaces
{
    using System.Collections.Generic;

    // Apply this marker interface only to aggregate root entities
    // Write repositories will only work with aggregate roots.
    public interface IEventSourcedAggregateRoot<out TId> : IAggregateRoot<TId>
    {
        public int Version { get; }

        void Load(IEnumerable<IDomainEvent> history);
    }
}