namespace JordiAragon.SharedKernel.Domain.Entities
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations.Schema;
    using Ardalis.GuardClauses;
    using JordiAragon.SharedKernel.Domain.Contracts.Interfaces;
    using JordiAragon.SharedKernel.Domain.ValueObjects;

    public abstract class BaseAggregateRoot<TId, TIdType> : BaseAuditableEntity<TId>, IDomainEventsContainer, IAggregateRoot<TId>
        where TId : BaseAggregateRootId<TIdType>
    {
        private readonly List<IDomainEvent> domainEvents = new();

        protected BaseAggregateRoot(TId id)
            : base(id)
        {
            this.Id = Guard.Against.Null(id, nameof(id));
        }

        // Required by EF.
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

        public void ClearDomainEvents() => this.domainEvents.Clear();

        protected void MarkAsDeleted()
        {
            this.IsDeleted = true;
        }

        protected void RegisterDomainEvent(IDomainEvent domainEvent) => this.domainEvents.Add(domainEvent);
    }
}