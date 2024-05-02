namespace JordiAragon.SharedKernel.Domain.Entities
{
    using System.Collections.Generic;
    using System.Linq;
    using JordiAragon.SharedKernel.Domain.Contracts.Interfaces;
    using JordiAragon.SharedKernel.Helpers;

    public abstract class BaseEventSourcedAggregateRoot<TId> : BaseAggregateRoot<TId>, IEventSourcedAggregateRoot<TId>
        where TId : class, IEntityId
    {
        protected BaseEventSourcedAggregateRoot(TId id)
            : base(id)
        {
            this.Version = Enumerable.Repeat((byte)0, 8).ToArray();
        }

        protected BaseEventSourcedAggregateRoot()
        {
        }

        public void Load(IEnumerable<IDomainEvent> history)
        {
            foreach (var @event in history)
            {
                this.When(@event);
                ConcurrencyVersionHelper.IncrementVersion(this.Version);
            }
        }
    }
}