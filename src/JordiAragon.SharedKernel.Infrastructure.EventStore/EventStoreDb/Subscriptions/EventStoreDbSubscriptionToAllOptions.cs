namespace JordiAragon.SharedKernel.Infrastructure.EventStore.EventStoreDb.Subscriptions
{
    using System;
    using global::EventStore.Client;
    using EventTypeFilter = global::EventStore.Client.EventTypeFilter;

    public class EventStoreDbSubscriptionToAllOptions
    {
        public Guid SubscriptionId { get; set; } = Guid.Empty;

        public SubscriptionFilterOptions FilterOptions { get; set; } =
            new(EventTypeFilter.ExcludeSystemEvents());

        public Action<EventStoreClientOperationOptions> ConfigureOperation { get; set; }

        public UserCredentials Credentials { get; set; }

        public bool ResolveLinkTos { get; set; }

        public bool IgnoreDeserializationErrors { get; set; } = true;
    }
}