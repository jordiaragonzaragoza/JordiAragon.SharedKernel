namespace JordiAragon.SharedKernel.Infrastructure.EntityFramework
{
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;
    using Ardalis.GuardClauses;
    using Ardalis.Specification;
    using JordiAragon.SharedKernel.Application.Contracts.Interfaces;
    using JordiAragon.SharedKernel.Contracts.Events;
    using JordiAragon.SharedKernel.Domain.Contracts.Interfaces;
    using JordiAragon.SharedKernel.Domain.Events.Services;

    public abstract class BaseDomainEventsDispatcher : IDomainEventsDispatcher
    {
        private readonly BaseContext context;
        private readonly IEventsDispatcherService eventDispatcherService;

        protected BaseDomainEventsDispatcher(
            BaseContext context,
            IEventsDispatcherService domainEventDispatcherService)
        {
            this.context = Guard.Against.Null(context, nameof(context));
            this.eventDispatcherService = Guard.Against.Null(domainEventDispatcherService, nameof(domainEventDispatcherService));
        }

        public virtual async Task DispatchDomainEventsAsync(CancellationToken cancellationToken = default)
        {
            var eventableEntities = this.context.ChangeTracker.Entries<IEventsContainer<IDomainEvent>>()
                            .Select(e => e.Entity)
                            .Where(entity => entity.Events.Any());

            if (!eventableEntities.Any())
            {
                return;
            }

            await this.eventDispatcherService.DispatchAndClearEventsAsync(eventableEntities, cancellationToken);
        }
    }
}