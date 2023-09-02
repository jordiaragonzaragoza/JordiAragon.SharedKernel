namespace JordiAragon.SharedKernel.Infrastructure.EventStore
{
    using System;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using Ardalis.GuardClauses;
    using global::EventStore.ClientAPI;
    using JordiAragon.SharedKernel.Domain.Contracts.Interfaces;
    using Newtonsoft.Json; // TODO: Change to System.Text.Json;

    public class EsAggregateStore : IAggregateStore
    {
        private readonly IEventStoreConnection connection;

        public EsAggregateStore(IEventStoreConnection connection) => this.connection = connection;

        public async Task SaveAsync<T, TId>(T aggregate)
            where T : IEventSourcedAggregateRoot<TId>
        {
            if (aggregate == null)
            {
                throw new ArgumentNullException(nameof(aggregate));
            }

            var changes = aggregate.Events
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
                return;
            }

            var streamName = GetStreamName<T, TId>(aggregate);

            await this.connection.AppendToStreamAsync(
                streamName,
                aggregate.Version,
                changes);

            aggregate.ClearEvents();
        }

        public async Task<T> LoadAsync<T, TId>(TId aggregateId)
            where T : IEventSourcedAggregateRoot<TId>
        {
            Guard.Against.Null(aggregateId, nameof(aggregateId));

            var stream = GetStreamName<T, TId>(aggregateId);

            // If this reflection causes performance issues, use a public constructors on aggregates.
            var aggregate = (T)Activator.CreateInstance(typeof(T), true);

            var page = await this.connection.ReadStreamEventsForwardAsync(
                stream, 0, 1024, false);

            aggregate.Load(page.Events.Select(resolvedEvent =>
            {
                var meta = JsonConvert.DeserializeObject<EventMetadata>(
                    Encoding.UTF8.GetString(resolvedEvent.Event.Metadata));
                var dataType = Type.GetType(meta.ClrType);
                var jsonData = Encoding.UTF8.GetString(resolvedEvent.Event.Data);
                var data = JsonConvert.DeserializeObject(jsonData, dataType);

                return (IDomainEvent)data;
            }));

            return aggregate;
        }

        public async Task<bool> ExistsAsync<T, TId>(TId aggregateId)
        {
            var stream = GetStreamName<T, TId>(aggregateId);
            var result = await this.connection.ReadEventAsync(stream, 1, false);

            return result.Status != EventReadStatus.NoStream;
        }

        private static byte[] Serialize(object data)
            => Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(data));

        private static string GetStreamName<T, TId>(TId aggregateId)
            => $"{typeof(T).Name}-{aggregateId}";

        private static string GetStreamName<T, TId>(T aggregate)
            where T : IEventSourcedAggregateRoot<TId>
            => $"{typeof(T).Name}-{aggregate.Id}";

        private sealed class EventMetadata
        {
            public string ClrType { get; set; }
        }
    }
}