namespace JordiAragon.SharedKernel.Application.Contracts.Interfaces
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;
    using JordiAragon.SharedKernel.Application.Contracts.IntegrationMessages.Interfaces;

    public interface IEventBus
    {
        Task PublishEventAsync<T>(T @event, CancellationToken cancellationToken = default)
            where T : class, IIntegrationEvent;

        Task SendCommandAsync<T>(T command, Uri endpointAddress, CancellationToken cancellationToken = default)
            where T : class, IIntegrationCommand;
    }
}