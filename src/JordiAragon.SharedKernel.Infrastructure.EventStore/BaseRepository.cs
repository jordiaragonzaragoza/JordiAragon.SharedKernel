namespace JordiAragon.SharedKernel.Infrastructure.EventStore
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;
    using Ardalis.GuardClauses;
    using global::EventStore.Client;
    using JordiAragon.SharedKernel.Contracts.DependencyInjection;
    using JordiAragon.SharedKernel.Domain.Contracts.Interfaces;
    using JordiAragon.SharedKernel.Infrastructure.EventStore.Serialization;

    public abstract class BaseRepository<TAggregate, TId> : IRepository<TAggregate, TId>, ISingletonDependency
        where TAggregate : class, IEventSourcedAggregateRoot<TId>
        where TId : class, IEntityId
    {
        protected BaseRepository(EventStoreClient eventStoreClient)
            => this.EventStoreClient = eventStoreClient;

        protected EventStoreClient EventStoreClient { get; private init; }

        public async Task<TAggregate> GetByIdAsync(TId id, CancellationToken cancellationToken = default)
        {
            Guard.Against.Null(id, nameof(id));

            var readResult = this.EventStoreClient.ReadStreamAsync(
                Direction.Forwards,
                StreamNameMapper.ToStreamId<TAggregate>(id),
                StreamPosition.Start,
                cancellationToken: cancellationToken);

            if (await readResult.ReadState.ConfigureAwait(false) == ReadState.StreamNotFound)
            {
                return null;
            }

            // If this reflection causes performance issues, use a public constructors on aggregates.
            var aggregate = (TAggregate)Activator.CreateInstance(typeof(TAggregate), true);

            var domainEvents = new List<IDomainEvent>();
            await foreach (ResolvedEvent resolvedEvent in readResult)
            {
                var domainEvent = SerializerHelper.Deserialize(resolvedEvent);
                domainEvents.Add(domainEvent);
            }

            aggregate.Load(domainEvents);

            return aggregate;
        }

        public Task<List<TAggregate>> ListAsync(CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException(); // TODO: Complete.
        }

        public Task<bool> AnyAsync(CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException(); // TODO: Complete.
        }

        public async Task<TAggregate> AddAsync(TAggregate aggregate, CancellationToken cancellationToken = default)
        {
            return await this.StoreAsync(aggregate, cancellationToken);
        }

        public async Task<IEnumerable<TAggregate>> AddRangeAsync(IEnumerable<TAggregate> entities, CancellationToken cancellationToken = default)
        {
            foreach (var entity in entities)
            {
                await this.StoreAsync(entity, cancellationToken);
            }

            return entities;
        }

        public async Task UpdateAsync(TAggregate aggregate, CancellationToken cancellationToken = default)
        {
            await this.StoreAsync(aggregate, cancellationToken);
        }

        public async Task UpdateRangeAsync(IEnumerable<TAggregate> entities, CancellationToken cancellationToken = default)
        {
            foreach (var entity in entities)
            {
                await this.StoreAsync(entity, cancellationToken);
            }
        }

        public async Task DeleteAsync(TAggregate aggregate, CancellationToken cancellationToken = default)
        {
            await this.StoreAsync(aggregate, cancellationToken);
        }

        public async Task DeleteRangeAsync(IEnumerable<TAggregate> aggregates, CancellationToken cancellationToken = default)
        {
            foreach (var entity in aggregates)
            {
                await this.StoreAsync(entity, cancellationToken);
            }
        }

        private async Task<TAggregate> StoreAsync(TAggregate aggregate, CancellationToken cancellationToken)
        {
            var events = aggregate.Events.Select(SerializerHelper.Serialize).ToArray();

            if (!events.Any())
            {
                return aggregate;
            }

            var streamName = StreamNameMapper.ToStreamId(typeof(TAggregate), aggregate.Id);

            await this.EventStoreClient.AppendToStreamAsync(
                streamName,
                StreamRevision.FromInt64(aggregate.Version),
                events,
                cancellationToken: cancellationToken);

            aggregate.ClearEvents();

            return aggregate;
        }
    }
}
