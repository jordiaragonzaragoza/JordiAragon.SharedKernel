namespace JordiAragon.SharedKernel.Domain.Entities
{
    using System.Collections.Generic;
    using JordiAragon.SharedKernel.Domain.Contracts.Interfaces;
    using JordiAragon.SharedKernel.Domain.ValueObjects;

    public abstract class BaseEventSourcedAggregateRoot<TId, TIdType> : BaseAggregateRoot<TId, TIdType>, IEventSourcedAggregateRoot<TId>
        where TId : BaseAggregateRootId<TIdType>
    {
        protected BaseEventSourcedAggregateRoot(TId id)
            : base(id)
        {
        }

        // Required by EF.
        protected BaseEventSourcedAggregateRoot()
        {
        }

        public int Version { get; private set; } = -1;

        public void Load(IEnumerable<IDomainEvent> history)
        {
            foreach (var @event in history)
            {
                this.When(@event);
                this.Version++;
            }
        }
    }
}