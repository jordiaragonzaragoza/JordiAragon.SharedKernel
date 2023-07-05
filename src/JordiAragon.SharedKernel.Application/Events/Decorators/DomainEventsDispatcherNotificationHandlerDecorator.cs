namespace JordiAragon.SharedKernel.Application.Events.Decorators
{
    using System.Threading;
    using System.Threading.Tasks;
    using JordiAragon.SharedKernel.Application.Contracts.Interfaces;
    using JordiAragon.SharedKernel.Domain.Contracts.Interfaces;

    public class DomainEventsDispatcherNotificationHandlerDecorator<T> : IDomainEventHandler<T>
        where T : IDomainEvent
    {
        private readonly IDomainEventHandler<T> decorated;
        private readonly IDomainEventsDispatcher domainEventsDispatcher;

        public DomainEventsDispatcherNotificationHandlerDecorator(
            IDomainEventsDispatcher domainEventsDispatcher,
            IDomainEventHandler<T> decorated)
        {
            this.domainEventsDispatcher = domainEventsDispatcher;
            this.decorated = decorated;
        }

        public async Task Handle(T notification, CancellationToken cancellationToken)
        {
            await this.decorated.Handle(notification, cancellationToken);

            await this.domainEventsDispatcher.DispatchDomainEventsAsync(cancellationToken);
        }
    }
}