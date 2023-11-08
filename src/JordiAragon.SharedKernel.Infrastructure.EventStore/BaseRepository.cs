namespace JordiAragon.SharedKernel.Infrastructure.EventStore
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading;
    using System.Threading.Tasks;
    using global::EventStore.ClientAPI;
    using JordiAragon.SharedKernel.Contracts.DependencyInjection;
    using JordiAragon.SharedKernel.Domain.Contracts.Interfaces;
    using Newtonsoft.Json;

    public abstract class BaseRepository<T, TId> : BaseReadRepository<T, TId>, IRepository<T, TId>, IScopedDependency
        where T : class, IEventSourcedAggregateRoot<TId>
    {
        protected BaseRepository(IEventStoreConnection connection)
            : base(connection)
        {
        }

        public async Task<T> AddAsync(T entity, CancellationToken cancellationToken = default)
        {
            var changes = entity.Events
                .Select(@event =>
                    new EventData(
                        eventId: @event.Id,
                        type: @event.GetType().Name,
                        isJson: true,
                        data: Serialize(@event),
                        metadata: Serialize(new EventMetadata
                        { ClrType = @event.GetType().AssemblyQualifiedName })))
                .ToArray();

            if (!changes.Any())
            {
                return entity;
            }

            var streamName = GetStreamName(entity);

            await this.Connection.AppendToStreamAsync(
                streamName,
                entity.Version,
                changes);

            entity.ClearEvents();

            return entity;
        }

        public Task<IEnumerable<T>> AddRangeAsync(IEnumerable<T> entities, CancellationToken cancellationToken = default)
        {
            throw new System.NotImplementedException();
        }

        public Task DeleteAsync(T entity, CancellationToken cancellationToken = default)
        {
            throw new System.NotImplementedException();
        }

        public Task DeleteRangeAsync(IEnumerable<T> entities, CancellationToken cancellationToken = default)
        {
            throw new System.NotImplementedException();
        }

        public Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            throw new System.NotImplementedException();
        }

        public Task UpdateAsync(T entity, CancellationToken cancellationToken = default)
        {
            throw new System.NotImplementedException();
        }

        public Task UpdateRangeAsync(IEnumerable<T> entities, CancellationToken cancellationToken = default)
        {
            throw new System.NotImplementedException();
        }

        private static byte[] Serialize(object data)
            => Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(data));

        private static string GetStreamName(T aggregate)
            => $"{typeof(T).Name}-{aggregate.Id}";

        private sealed class EventMetadata
        {
            public string ClrType { get; set; }
        }
    }
}
