namespace JordiAragon.SharedKernel.Infrastructure.EventStore.AssemblyConfiguration
{
    using global::EventStore.Client;
    using JordiAragon.SharedKernel.Helpers;
    using JordiAragon.SharedKernel.Infrastructure.EventStore.EventStoreDb;
    using JordiAragon.SharedKernel.Infrastructure.EventStore.EventStoreDb.Subscriptions;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Logging;

    public static class ConfigureServices
    {
        public static IServiceCollection AddSharedKernelEventStoreServices(this IServiceCollection serviceCollection, IConfiguration configuration)
        {
            serviceCollection
                .AddEventStoreDB(configuration.GetRequiredConfiguration<EventStoreDbOptions>(EventStoreDbOptions.Section))
                .AddEventStoreDBSubscriptionToAll();

            return serviceCollection;
        }

        private static IServiceCollection AddEventStoreDB(this IServiceCollection serviceCollection, EventStoreDbOptions eventStoreDBConfig)
            => serviceCollection
                .AddSingleton(EventTypeMapper.Instance) // TODO: Register with autofac.
                .AddSingleton(new EventStoreClient(EventStoreClientSettings.Create(eventStoreDBConfig.ConnectionString)))
                .AddTransient<EventStoreDbSubscriptionToAll, EventStoreDbSubscriptionToAll>(); // TODO: Register with autofac.

        private static IServiceCollection AddEventStoreDBSubscriptionToAll(
            this IServiceCollection serviceCollection,
            EventStoreDbSubscriptionToAllOptions subscriptionOptions = null)
        {
            return serviceCollection.AddHostedService(serviceProvider =>
            {
                var serviceScopeFactory = serviceProvider.GetRequiredService<IServiceScopeFactory>();
                var logger = serviceProvider.GetRequiredService<ILogger<BackgroundWorker>>();
                var eventStoreDBSubscriptionToAll = serviceProvider.GetRequiredService<EventStoreDbSubscriptionToAll>();

                return new BackgroundWorker(
                    logger,
                    cancellationToken =>
                        eventStoreDBSubscriptionToAll.SubscribeToAllAsync(
                            serviceScopeFactory,
                            subscriptionOptions ?? new EventStoreDbSubscriptionToAllOptions(),
                            cancellationToken));
            });
        }
    }
}