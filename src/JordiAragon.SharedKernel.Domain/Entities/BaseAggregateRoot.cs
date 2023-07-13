﻿namespace JordiAragon.SharedKernel.Domain.Entities
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations.Schema;
    using Ardalis.GuardClauses;
    using JordiAragon.SharedKernel.Contracts.Events;
    using JordiAragon.SharedKernel.Domain.Contracts.Interfaces;
    using JordiAragon.SharedKernel.Domain.ValueObjects;

    public abstract class BaseAggregateRoot<TId, TIdType> : BaseAuditableEntity<TId>, IAggregateRoot<TId>, IEventsContainer<IDomainEvent>, ISoftDelete
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

        public bool IsDeleted { get; private set; }

        [NotMapped]
        public IEnumerable<IDomainEvent> Events => this.domainEvents.AsReadOnly();

        public void ClearEvents() => this.domainEvents.Clear();

        protected void MarkAsDeleted() // TODO: Complete.
        {
            this.IsDeleted = true;
        }

        protected void RegisterDomainEvent(IDomainEvent domainEvent) => this.domainEvents.Add(domainEvent);
    }
}