namespace JordiAragon.SharedKernel.Domain.Entities
{
    using System.Collections.Generic;
    using JordiAragon.SharedKernel.Domain.Contracts.Interfaces;
    using JordiAragon.SharedKernel.Helpers;

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
            this.Version = ConcurrencyVersionHelper.InitializeMinusOneByteVersion();

            foreach (var @event in history)
            {
                this.When(@event);
                ConcurrencyVersionHelper.IncrementByteVersion(this.Version);
            }
        }
    }
}