namespace JordiAragon.SharedKernel.Infrastructure.EntityFramework
{
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;
    using Ardalis.Specification;
    using JordiAragon.SharedKernel.Application.Contracts.Interfaces;
    using JordiAragon.SharedKernel.Domain.Contracts.Interfaces;
    using JordiAragon.SharedKernel.Domain.Events.Services;

    public abstract class BaseDomainEventsDispatcher : IDomainEventsDispatcher
    {
        private readonly BaseContext context;
        private readonly IDomainEventsDispatcherService domainEventDispatcherService;

        protected BaseDomainEventsDispatcher(
            BaseContext context,
            IDomainEventsDispatcherService domainEventDispatcherService)
        {
            this.context = context;
            this.domainEventDispatcherService = domainEventDispatcherService;
        }

        public virtual async Task DispatchDomainEventsAsync(CancellationToken cancellationToken = default)
        {
            var eventableEntities = this.context.ChangeTracker.Entries<IDomainEventsContainer>()
                            .Select(e => e.Entity)
                            .Where(entity => entity.DomainEvents.Any());

            if (!eventableEntities.Any())
            {
                return;
            }

            // Note: If an unhandled exception occurs, all the saved changes will be rolled back
            // by the UnitOfWorkBehaviour. All the operations related to a domain event finish
            // successfully or none of them do.
            // Reference: https://docs.microsoft.com/en-us/dotnet/architecture/microservices/microservice-ddd-cqrs-patterns/domain-events-design-implementation#what-is-a-domain-event
            await this.domainEventDispatcherService.DispatchAndClearEventsAsync(eventableEntities, cancellationToken);
        }
    }
}