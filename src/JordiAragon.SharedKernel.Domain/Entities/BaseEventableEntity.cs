namespace JordiAragon.SharedKernel.Domain.Entities
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations.Schema;
    using JordiAragon.SharedKernel.Domain.Contracts.Interfaces;

    public abstract class BaseEventableEntity<TId> : BaseEntity<TId>
    {
        private readonly List<IDomainEvent> domainEvents = new();

        protected BaseEventableEntity(TId id)
            : base(id)
        {
        }

        [NotMapped]
        public IEnumerable<IDomainEvent> DomainEvents => this.domainEvents.AsReadOnly();

        internal void ClearDomainEvents() => this.domainEvents.Clear();

        protected void RegisterDomainEvent(IDomainEvent domainEvent) => this.domainEvents.Add(domainEvent);
    }
}