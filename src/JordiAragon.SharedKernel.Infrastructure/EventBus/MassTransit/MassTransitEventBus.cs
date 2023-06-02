namespace JordiAragon.SharedKernel.Infrastructure.EventBus.MassTransit
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

    public class MassTransitEventBus : IEventBus, ITransientDependency
    {
        private readonly IPublishEndpoint publishEndPoint;
        private readonly ISendEndpointProvider sendEndpointProvider;
        private readonly ILogger<MassTransitEventBus> logger;
        private readonly IDateTime dateTime;

        public MassTransitEventBus(
            IPublishEndpoint publishEndPoint,
            ISendEndpointProvider sendEndpointProvider,
            ILogger<MassTransitEventBus> logger,
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

            await this.publishEndPoint.Publish(@event, cancellationToken); // TODO: See MassTransit Outbox EF.

            this.logger.LogInformation("Published Integration event {Event} {Id}", @event.GetType().Name, @event.Id);
        }

        public async Task SendCommandAsync<T>(T command, Uri endpointAddress, CancellationToken cancellationToken = default)
            where T : class, IIntegrationCommand
        {
            var endpoint = await this.sendEndpointProvider.GetSendEndpoint(endpointAddress);

            command.DatePublishedOnUtc = this.dateTime.UtcNow;

            await endpoint.Send(command, cancellationToken); // TODO: See MassTransit Outbox EF.

            this.logger.LogInformation("Sent Integration command: {Command} {Id}", command.GetType().Name, command.Id);
        }
    }
}