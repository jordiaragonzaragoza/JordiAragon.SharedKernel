namespace JordiAragon.SharedKernel.Infrastructure.EventStore
{
    using System.Threading;
    using System.Threading.Tasks;
    using global::EventStore.ClientAPI;
    using JordiAragon.SharedKernel.Contracts.DependencyInjection;
    using Microsoft.Extensions.Hosting;

    public class HostedService : IHostedService, ISingletonDependency
    {
        private readonly IEventStoreConnection esConnection;

        public HostedService(IEventStoreConnection esConnection)
        {
            this.esConnection = esConnection;
        }

        public Task StartAsync(CancellationToken cancellationToken)
            => this.esConnection.ConnectAsync();

        public Task StopAsync(CancellationToken cancellationToken)
        {
            this.esConnection.Close();
            return Task.CompletedTask;
        }
    }
}