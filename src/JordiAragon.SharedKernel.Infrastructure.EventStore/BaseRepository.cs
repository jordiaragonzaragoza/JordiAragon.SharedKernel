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
    using JordiAragon.SharedKernel.Domain.Entities;
    using JordiAragon.SharedKernel.Infrastructure.EventStore.Serialization;
    using Microsoft.Extensions.Logging;

    public abstract class BaseRepository<TAggregate, TId> : IRepository<TAggregate, TId>, ISingletonDependency
        where TAggregate : BaseEventSourcedAggregateRoot<TId>
        where TId : class, IEntityId
    {
        private readonly ILogger<BaseRepository<TAggregate, TId>> logger;

        protected BaseRepository(
            EventStoreClient eventStoreClient,
            ILogger<BaseRepository<TAggregate, TId>> logger)
        {
            this.EventStoreClient = Guard.Against.Null(eventStoreClient, nameof(eventStoreClient));
            this.logger = Guard.Against.Null(logger, nameof(logger));
        }

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

            // If this reflection causes performance issues, use a public constructors on aggregates if its required.
            var aggregate = (TAggregate)Activator.CreateInstance(typeof(TAggregate), true);

            var domainEvents = new List<IDomainEvent>();
            await foreach (var resolvedEvent in readResult)
            {
                var domainEvent = SerializerHelper.Deserialize(resolvedEvent);
                domainEvents.Add(domainEvent);
            }

            this.logger.LogInformation("Loading events for the aggregate: {Aggregate}", aggregate.ToString());

            aggregate.Load(domainEvents);

            return aggregate;
        }

        public async Task<TAggregate> AddAsync(TAggregate aggregate, CancellationToken cancellationToken = default)
        {
            return await this.StoreAsync(aggregate, cancellationToken);
        }

        public async Task UpdateAsync(TAggregate aggregate, CancellationToken cancellationToken = default)
        {
            await this.StoreAsync(aggregate, cancellationToken);
        }

        public async Task DeleteAsync(TAggregate aggregate, CancellationToken cancellationToken = default)
        {
            await this.StoreAsync(aggregate, cancellationToken);
        }

        private async Task<TAggregate> StoreAsync(TAggregate aggregate, CancellationToken cancellationToken)
        {
            var events = aggregate.Events.Select(SerializerHelper.Serialize).ToArray();

            if (!events.Any())
            {
                return aggregate;
            }

            var streamName = StreamNameMapper.ToStreamId<TAggregate>(aggregate.Id);
            var nextVersion = StreamRevision.FromInt64(aggregate.Version); // TODO: Review if its correct.

            foreach (var @event in events)
            {
                this.logger.LogInformation("Persisting event: {Event}", @event.ToString());
            }

            await this.EventStoreClient.AppendToStreamAsync(
                streamName,
                nextVersion,
                events,
                cancellationToken: cancellationToken);

            aggregate.ClearEvents();

            return aggregate;
        }
    }
}
