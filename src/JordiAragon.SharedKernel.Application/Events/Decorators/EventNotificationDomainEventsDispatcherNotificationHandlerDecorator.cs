namespace JordiAragon.SharedKernel.Application.Events.Decorators
{
    using System.Threading;
    using System.Threading.Tasks;
    using Ardalis.GuardClauses;
    using JordiAragon.SharedKernel.Application.Contracts.Interfaces;
    using JordiAragon.SharedKernel.Contracts.Events;
    using MediatR;

    public class EventNotificationDomainEventsDispatcherNotificationHandlerDecorator<TEventNotification> : INotificationHandlerDecorator<TEventNotification>
        where TEventNotification : IEventNotification
    {
        private readonly INotificationHandler<TEventNotification> decoratedHandler;
        private readonly IDomainEventsDispatcher domainEventsDispatcher;

        public EventNotificationDomainEventsDispatcherNotificationHandlerDecorator(
            IDomainEventsDispatcher domainEventsDispatcher,
            INotificationHandler<TEventNotification> decoratedHandler)
        {
            this.domainEventsDispatcher = Guard.Against.Null(domainEventsDispatcher, nameof(domainEventsDispatcher));
            this.decoratedHandler = Guard.Against.Null(decoratedHandler, nameof(decoratedHandler));
        }

        public INotificationHandler<TEventNotification> DecoratedHandler
            => this.decoratedHandler;

        public async Task Handle(TEventNotification notification, CancellationToken cancellationToken)
        {
            await this.decoratedHandler.Handle(notification, cancellationToken).ConfigureAwait(true);

            await this.domainEventsDispatcher.DispatchDomainEventsAsync(cancellationToken);
        }
    }
}