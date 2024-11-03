﻿namespace JordiAragonZaragoza.SharedKernel.Domain.Entities
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations.Schema;
    using Ardalis.Result;
    using JordiAragonZaragoza.SharedKernel.Domain.Contracts.Interfaces;

    public abstract class BaseAggregateRoot<TId> : BaseEntity<TId>, IAggregateRoot<TId>
       where TId : class, IEntityId
    {
        private readonly List<IDomainEvent> domainEvents = [];

        protected BaseAggregateRoot(TId id)
            : base(id)
        {
        }

        // Required by EF.
        protected BaseAggregateRoot()
        {
        }

        public uint Version { get; protected set; }

        [NotMapped]
        public IEnumerable<IDomainEvent> Events => this.domainEvents.AsReadOnly();

        public void ClearEvents() => this.domainEvents.Clear();

        protected void Apply(IDomainEvent domainEvent)
        {
            this.When(domainEvent);
            this.EnsureValidState();
            this.domainEvents.Add(domainEvent);
        }

        /// <summary>
        /// When is used to project events into the state of the aggregate.
        /// This means that it is responsible for applying the changes described in each event to the current state of the aggregate.
        /// </summary>
        /// <param name="domainEvent">The domain event to apply.</param>
        protected abstract void When(IDomainEvent domainEvent);

        /// <summary>
        /// This method checks that in any situation, the state of the entity is valid and if it is not, it will return an error result.
        /// When we call this method from any operation method, we can be sure that no matter what we try to do,
        /// our entity will always be in a valid state or the caller will get an error result.
        /// </summary>
        protected abstract void EnsureValidState();
    }
}