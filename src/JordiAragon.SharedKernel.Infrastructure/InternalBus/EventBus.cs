namespace JordiAragon.SharedKernel.Infrastructure.InternalBus
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;
    using global::MediatR;
    using JordiAragon.SharedKernel.Domain.Contracts.Interfaces;
    using JordiAragon.SharedKernel.Contracts.Events;

    public class EventBus : IEventBus
    {
        private readonly IPublisher publisher;

        public EventBus(IPublisher publisher)
        {
            this.publisher = publisher ?? throw new ArgumentNullException(nameof(publisher));
        }

        public Task PublishAsync(IEvent @event, CancellationToken cancellationToken = default)
            => this.publisher.Publish(@event, cancellationToken);

        public Task PublishAsync(IEventNotification eventNotification, CancellationToken cancellationToken = default)
            => this.publisher.Publish(eventNotification, cancellationToken);
    }
}