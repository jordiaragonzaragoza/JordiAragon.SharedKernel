namespace JordiAragonZaragoza.SharedKernel.Domain.Entities
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

        protected Result Apply(IDomainEvent domainEvent)
        {
            var result = this.When(domainEvent);
            if (!result.IsSuccess)
            {
                return result;
            }

            this.EnsureValidState();
            this.domainEvents.Add(domainEvent);

            return result;
        }

        protected abstract Result When(IDomainEvent domainEvent);

        protected abstract void EnsureValidState();
    }
}