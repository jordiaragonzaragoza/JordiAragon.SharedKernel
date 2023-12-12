namespace JordiAragon.SharedKernel.Infrastructure
{
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;
    using Ardalis.GuardClauses;
    using JordiAragon.SharedKernel.Application.Contracts.Interfaces;
    using JordiAragon.SharedKernel.Domain.Events.Services;
    using JordiAragon.SharedKernel.Infrastructure.Interfaces;

    public class DomainEventsDispatcher : IDomainEventsDispatcher
    {
        private readonly IEventsDispatcherService eventDispatcherService;
        private readonly IWriteStore writeStore;
        private readonly IEventStore eventStore;

        public DomainEventsDispatcher(
            IWriteStore writeStore,
            IEventStore eventStore,
            IEventsDispatcherService domainEventDispatcherService)
        {
            this.writeStore = Guard.Against.Null(writeStore, nameof(writeStore));
            this.eventStore = Guard.Against.Null(eventStore, nameof(eventStore));
            this.eventDispatcherService = Guard.Against.Null(domainEventDispatcherService, nameof(domainEventDispatcherService));
        }

        public virtual async Task DispatchDomainEventsAsync(CancellationToken cancellationToken = default)
        {
            var writeStoreEventableEntities = this.writeStore.EventableEntities;

            var eventStoreEventableEntities = this.eventStore.EventableEntities;

            var eventableEntities = writeStoreEventableEntities.Concat(eventStoreEventableEntities);

            if (!eventableEntities.Any())
            {
                return;
            }

            await this.eventDispatcherService.DispatchAndClearEventsAsync(eventableEntities, cancellationToken);
        }
    }
}