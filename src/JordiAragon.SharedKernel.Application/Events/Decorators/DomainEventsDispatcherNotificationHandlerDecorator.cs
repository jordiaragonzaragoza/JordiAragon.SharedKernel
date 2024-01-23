namespace JordiAragon.SharedKernel.Application.Events.Decorators
{
    using System.Threading;
    using System.Threading.Tasks;
    using Ardalis.GuardClauses;
    using JordiAragon.SharedKernel.Application.Contracts.Interfaces;
    using JordiAragon.SharedKernel.Domain.Contracts.Interfaces;
    using MediatR;

    public class DomainEventsDispatcherNotificationHandlerDecorator<TDomainEvent> : INotificationHandlerDecorator<TDomainEvent>
        where TDomainEvent : IDomainEvent
    {
        private readonly INotificationHandler<TDomainEvent> decoratedHandler;
        private readonly IDomainEventsDispatcher domainEventsDispatcher;

        public DomainEventsDispatcherNotificationHandlerDecorator(
            IDomainEventsDispatcher domainEventsDispatcher,
            INotificationHandler<TDomainEvent> decoratedHandler)
        {
            this.domainEventsDispatcher = Guard.Against.Null(domainEventsDispatcher, nameof(domainEventsDispatcher));
            this.decoratedHandler = Guard.Against.Null(decoratedHandler, nameof(decoratedHandler));
        }

        public INotificationHandler<TDomainEvent> DecoratedHandler
            => this.decoratedHandler;

        public async Task Handle(TDomainEvent notification, CancellationToken cancellationToken)
        {
            await this.decoratedHandler.Handle(notification, cancellationToken).ConfigureAwait(true);

            await this.domainEventsDispatcher.DispatchDomainEventsAsync(cancellationToken);
        }
    }
}