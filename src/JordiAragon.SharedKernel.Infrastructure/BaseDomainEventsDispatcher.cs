namespace JordiAragon.SharedKernel.Infrastructure
{
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;
    using Ardalis.GuardClauses;
    using JordiAragon.SharedKernel.Application.Contracts.Interfaces;
    using JordiAragon.SharedKernel.Contracts.DependencyInjection;
    using JordiAragon.SharedKernel.Domain.Events.Services;
    using JordiAragon.SharedKernel.Infrastructure.Interfaces;

    public abstract class BaseDomainEventsDispatcher : IDomainEventsDispatcher, IScopedDependency
    {
        private readonly IEventsDispatcherService eventDispatcherService;
        private readonly IBusinessModelStore businessModelStore;
        private readonly IEventStore eventStore;

        protected BaseDomainEventsDispatcher(
            IBusinessModelStore businessModelStore,
            IEventStore eventStore,
            IEventsDispatcherService domainEventDispatcherService)
        {
            this.businessModelStore = Guard.Against.Null(businessModelStore, nameof(businessModelStore));
            this.eventStore = Guard.Against.Null(eventStore, nameof(eventStore));
            this.eventDispatcherService = Guard.Against.Null(domainEventDispatcherService, nameof(domainEventDispatcherService));
        }

        public async Task DispatchDomainEventsAsync(CancellationToken cancellationToken = default)
        {
            var businessModelStoreEventableEntities = this.businessModelStore.EventableEntities;

            var eventStoreEventableEntities = this.eventStore.EventableEntities;

            var eventableEntities = businessModelStoreEventableEntities.Concat(eventStoreEventableEntities);

            if (!eventableEntities.Any())
            {
                return;
            }

            await this.eventDispatcherService.DispatchEventsAsync(eventableEntities, cancellationToken);
        }
    }
}