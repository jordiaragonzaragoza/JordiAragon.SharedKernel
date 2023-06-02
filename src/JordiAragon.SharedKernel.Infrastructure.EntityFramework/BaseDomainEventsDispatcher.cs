namespace JordiAragon.SharedKernel.Infrastructure.EntityFramework
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;
    using JordiAragon.SharedKernel.Application.Contracts.Interfaces;
    using JordiAragon.SharedKernel.Domain.Contracts.Interfaces;
    using JordiAragon.SharedKernel.Domain.Entities;
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
            // Deferred execution,
            // When the DB transaction ends, all registered events (in-process) will be executed.
            var eventableEntities = new List<dynamic>();

            foreach (var entity in this.context.ChangeTracker.Entries().Select(e => e.Entity))
            {
                var baseEventableEntityType = typeof(BaseEventableEntity<>);

                Type entityType = entity.GetType();

                var idProperty = entityType.GetProperty("Id");
                if (idProperty != null)
                {
                    Type[] typeArgs = { idProperty.PropertyType };

                    Type auditableEntityType = baseEventableEntityType.MakeGenericType(typeArgs);

                    if (auditableEntityType.IsAssignableFrom(entityType))
                    {
                        dynamic eventableEntity = entity;
                        if (((IEnumerable<IDomainEvent>)eventableEntity.DomainEvents).Any())
                        {
                            eventableEntities.Add(eventableEntity);
                        }
                    }
                }
            }

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