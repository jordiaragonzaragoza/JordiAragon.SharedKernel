namespace JordiAragon.SharedKernel.Domain.Entities
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations.Schema;
    using JordiAragon.SharedKernel.Domain.Contracts.Interfaces;

    public abstract class BaseAggregateRoot<TId, TIdType> : BaseEntity<TId>, IAggregateRoot<TId>
        where TId : BaseAggregateRootId<TIdType>
    {
        private readonly List<IDomainEvent> domainEvents = new();

        protected BaseAggregateRoot(TId id)
            : base(id)
        {
        }

        // Required by EF
        protected BaseAggregateRoot()
        {
        }

        public new BaseAggregateRootId<TIdType> Id { get; protected set; }

        /// <summary>
        /// Gets a value indicating whether indicates whether this aggregate is logically deleted.
        /// </summary>
        public bool IsDeleted { get; private set; }

        [NotMapped]
        public IEnumerable<IDomainEvent> DomainEvents => this.domainEvents.AsReadOnly();

        internal void ClearDomainEvents() => this.domainEvents.Clear();

        protected void MarkAsDeleted()
        {
            this.IsDeleted = true;
        }

        protected void RegisterDomainEvent(IDomainEvent domainEvent) => this.domainEvents.Add(domainEvent);
    }
}