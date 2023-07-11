namespace JordiAragon.SharedKernel.Application.Events.Decorators
{
    using System.Threading;
    using System.Threading.Tasks;
    using Ardalis.GuardClauses;
    using JordiAragon.SharedKernel.Application.Contracts.Interfaces;
    using JordiAragon.SharedKernel.Domain.Contracts.Interfaces;
    using MediatR;

    public class DomainEventsDispatcherNotificationHandlerDecorator<TDomainEvent> : INotificationHandler<TDomainEvent>
        where TDomainEvent : IDomainEvent
    {
        private readonly INotificationHandler<TDomainEvent> decorated;
        private readonly IDomainEventsDispatcher domainEventsDispatcher;

        public DomainEventsDispatcherNotificationHandlerDecorator(
            IDomainEventsDispatcher domainEventsDispatcher,
            INotificationHandler<TDomainEvent> decorated)
        {
            this.domainEventsDispatcher = Guard.Against.Null(domainEventsDispatcher, nameof(domainEventsDispatcher));
            this.decorated = Guard.Against.Null(decorated, nameof(decorated));
        }

        public async Task Handle(TDomainEvent notification, CancellationToken cancellationToken)
        {
            await this.decorated.Handle(notification, cancellationToken).ConfigureAwait(true);

            await this.domainEventsDispatcher.DispatchDomainEventsAsync(cancellationToken);
        }
    }
}