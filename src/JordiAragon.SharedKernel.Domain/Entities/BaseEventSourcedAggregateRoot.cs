namespace JordiAragon.SharedKernel.Domain.Entities
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations.Schema;
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

        [NotMapped]
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