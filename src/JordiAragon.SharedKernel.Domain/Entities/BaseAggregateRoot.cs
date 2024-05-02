namespace JordiAragon.SharedKernel.Domain.Entities
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations.Schema;
    using JordiAragon.SharedKernel.Domain.Contracts.Interfaces;

    public abstract class BaseAggregateRoot<TId> : BaseEntity<TId>, IAggregateRoot<TId>
       where TId : class, IEntityId
    {
        private readonly List<IDomainEvent> domainEvents = new();

        protected BaseAggregateRoot(TId id)
            : base(id)
        {
        }

        // Required by EF.
        protected BaseAggregateRoot()
        {
        }

        // This allows optiministic concurrency
        public byte[] Version { get; protected set; }

        [NotMapped]
        public IEnumerable<IDomainEvent> Events => this.domainEvents.AsReadOnly();

        public void ClearEvents() => this.domainEvents.Clear();

        protected void Apply(IDomainEvent domainEvent)
        {
            this.When(domainEvent);
            this.EnsureValidState();
            this.domainEvents.Add(domainEvent);
        }

        protected abstract void When(IDomainEvent domainEvent);

        protected abstract void EnsureValidState();
    }
}