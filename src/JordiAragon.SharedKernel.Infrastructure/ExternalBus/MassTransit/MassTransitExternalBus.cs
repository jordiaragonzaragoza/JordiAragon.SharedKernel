namespace JordiAragon.SharedKernel.Infrastructure.ExternalBus.MassTransit
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;
    using global::MassTransit;
    using JordiAragon.SharedKernel.Application.Contracts.IntegrationMessages.Interfaces;
    using JordiAragon.SharedKernel.Application.Contracts.Interfaces;
    using JordiAragon.SharedKernel.Contracts.DependencyInjection;
    using JordiAragon.SharedKernel.Domain.Contracts.Interfaces;
    using Microsoft.Extensions.Logging;

    public class MassTransitExternalBus : IExternalBus, ITransientDependency
    {
        private readonly IPublishEndpoint publishEndPoint;
        private readonly ISendEndpointProvider sendEndpointProvider;
        private readonly ILogger<MassTransitExternalBus> logger;
        private readonly IDateTime dateTime;

        public MassTransitExternalBus(
            IPublishEndpoint publishEndPoint,
            ISendEndpointProvider sendEndpointProvider,
            ILogger<MassTransitExternalBus> logger,
            IDateTime dateTime)
        {
            this.publishEndPoint = publishEndPoint ?? throw new ArgumentNullException(nameof(publishEndPoint));
            this.sendEndpointProvider = sendEndpointProvider ?? throw new ArgumentNullException(nameof(sendEndpointProvider));
            this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
            this.dateTime = dateTime ?? throw new ArgumentNullException(nameof(dateTime));
        }

        public async Task PublishEventAsync<T>(T @event, CancellationToken cancellationToken = default)
            where T : class, IIntegrationEvent
        {
            @event.DatePublishedOnUtc = this.dateTime.UtcNow;

            await this.publishEndPoint.Publish(@event, cancellationToken);

            this.logger.LogInformation("Published Integration event {Event} {Id}", @event.GetType().Name, @event.Id);
        }

        public async Task SendCommandAsync<T>(T command, Uri endpointAddress, CancellationToken cancellationToken = default)
            where T : class, IIntegrationCommand
        {
            var endpoint = await this.sendEndpointProvider.GetSendEndpoint(endpointAddress);

            command.DatePublishedOnUtc = this.dateTime.UtcNow;

            await endpoint.Send(command, cancellationToken);

            this.logger.LogInformation("Sent Integration command: {Command} {Id}", command.GetType().Name, command.Id);
        }
    }
}