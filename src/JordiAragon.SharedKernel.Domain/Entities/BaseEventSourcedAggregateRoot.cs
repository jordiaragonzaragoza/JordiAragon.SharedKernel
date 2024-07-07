namespace JordiAragon.SharedKernel.Domain.Entities
{
    using System.Collections.Generic;
    using JordiAragon.SharedKernel.Domain.Contracts.Interfaces;

    public abstract class BaseEventSourcedAggregateRoot<TId> : BaseAggregateRoot<TId>, IEventSourcedAggregateRoot<TId>
        where TId : class, IEntityId
    {
        protected BaseEventSourcedAggregateRoot(TId id)
            : base(id)
        {
        }

        protected BaseEventSourcedAggregateRoot()
        {
        }

        public void Load(IEnumerable<IDomainEvent> history)
        {
            this.Version = uint.MaxValue;

            foreach (var @event in history)
            {
                this.When(@event);
                this.Version++;
            }
        }
    }
}