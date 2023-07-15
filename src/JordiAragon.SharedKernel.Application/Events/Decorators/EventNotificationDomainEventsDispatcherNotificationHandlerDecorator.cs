namespace JordiAragon.SharedKernel.Application.Events.Decorators
{
    using System.Threading;
    using System.Threading.Tasks;
    using Ardalis.GuardClauses;
    using JordiAragon.SharedKernel.Application.Contracts.Interfaces;
    using JordiAragon.SharedKernel.Contracts.Events;
    using MediatR;

    public class EventNotificationDomainEventsDispatcherNotificationHandlerDecorator<TEventNotification> : INotificationHandler<TEventNotification>
        where TEventNotification : IEventNotification
    {
        private readonly INotificationHandler<TEventNotification> decorated;
        private readonly IDomainEventsDispatcher domainEventsDispatcher;

        public EventNotificationDomainEventsDispatcherNotificationHandlerDecorator(
            IDomainEventsDispatcher domainEventsDispatcher,
            INotificationHandler<TEventNotification> decorated)
        {
            this.domainEventsDispatcher = Guard.Against.Null(domainEventsDispatcher, nameof(domainEventsDispatcher));
            this.decorated = Guard.Against.Null(decorated, nameof(decorated));
        }

        public async Task Handle(TEventNotification notification, CancellationToken cancellationToken)
        {
            await this.decorated.Handle(notification, cancellationToken).ConfigureAwait(true);

            await this.domainEventsDispatcher.DispatchDomainEventsAsync(cancellationToken);
        }
    }
}