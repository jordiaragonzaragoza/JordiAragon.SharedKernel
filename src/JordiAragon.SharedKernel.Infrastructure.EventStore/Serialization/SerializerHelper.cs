namespace JordiAragon.SharedKernel.Infrastructure.EventStore.Serialization
{
    using System;
    using System.Text;
    using System.Text.Json;
    using global::EventStore.Client;
    using JordiAragon.SharedKernel.Domain.Contracts.Interfaces;
    using JordiAragon.SharedKernel.Infrastructure.EventStore.Events;

    public static class SerializerHelper
    {
        public static EventData Serialize(IDomainEvent @event)
        {
            return new EventData(
                                    eventId: Uuid.FromGuid(@event.Id),
                                    type: @event.GetType().Name, // TODO: Review.
                                    data: Encoding.UTF8.GetBytes(JsonSerializer.Serialize(@event)),
                                    metadata: Encoding.UTF8.GetBytes(JsonSerializer.Serialize(new EventMetadata { ClrType = @event.GetType().AssemblyQualifiedName })));
        }

        public static IDomainEvent Deserialize(ResolvedEvent resolvedEvent)
        {
            var meta = JsonSerializer.Deserialize<EventMetadata>(
                    Encoding.UTF8.GetString(resolvedEvent.Event.Metadata.ToArray()));

            var dataType = Type.GetType(meta.ClrType);
            var jsonData = Encoding.UTF8.GetString(resolvedEvent.Event.Data.ToArray());

            var data = JsonSerializer.Deserialize(jsonData, dataType);

            return (IDomainEvent)data;
        }
    }
}