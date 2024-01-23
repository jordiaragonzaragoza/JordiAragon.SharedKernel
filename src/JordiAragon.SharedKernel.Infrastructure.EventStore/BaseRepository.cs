namespace JordiAragon.SharedKernel.Infrastructure.EventStore
{
    using System.Threading;
    using System.Threading.Tasks;
    using Ardalis.GuardClauses;
    using JordiAragon.SharedKernel.Contracts.DependencyInjection;
    using JordiAragon.SharedKernel.Domain.Contracts.Interfaces;
    using JordiAragon.SharedKernel.Domain.Entities;

    public abstract class BaseRepository<TAggregate, TId> : IRepository<TAggregate, TId>, IScopedDependency
        where TAggregate : BaseEventSourcedAggregateRoot<TId>
        where TId : class, IEntityId
    {
        private readonly IEventStore eventStore;
        private TAggregate currentAggregate;

        protected BaseRepository(IEventStore eventStore)
        {
            this.eventStore = Guard.Against.Null(eventStore, nameof(eventStore));
        }

        public async Task<TAggregate> GetByIdAsync(TId id, CancellationToken cancellationToken = default)
        {
            Guard.Against.Null(id, nameof(id));

            if (this.currentAggregate is not null)
            {
                return this.currentAggregate;
            }

            this.currentAggregate = await this.eventStore.LoadAggregateAsync<TAggregate, TId>(id, cancellationToken);

            return this.currentAggregate;
        }

        public Task<TAggregate> AddAsync(TAggregate aggregate, CancellationToken cancellationToken = default)
        {
            return Task.FromResult(this.Store(aggregate));
        }

        public Task UpdateAsync(TAggregate aggregate, CancellationToken cancellationToken = default)
        {
            return Task.FromResult(this.Store(aggregate));
        }

        public Task DeleteAsync(TAggregate aggregate, CancellationToken cancellationToken = default)
        {
            return Task.FromResult(this.Store(aggregate));
        }

        private TAggregate Store(TAggregate aggregate)
        {
            this.eventStore.AppendChanges<TAggregate, TId>(aggregate);

            return aggregate;
        }
    }
}
