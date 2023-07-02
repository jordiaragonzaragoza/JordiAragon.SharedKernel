namespace JordiAragon.SharedKernel.Domain.Events.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;
    using Autofac;
    using Autofac.Core;
    using JordiAragon.SharedKernel.Contracts.Outbox;
    using JordiAragon.SharedKernel.Domain.Contracts.Interfaces;
    using MediatR;
    using Microsoft.Extensions.Logging;

    public class DomainEventsDispatcherService : IDomainEventsDispatcherService
    {
        private readonly IPublisher mediator;
        private readonly ILifetimeScope scope;
        private readonly IOutboxService outboxService;
        private readonly ILogger<DomainEventsDispatcherService> logger;

        public DomainEventsDispatcherService(
            IPublisher mediator,
            ILifetimeScope scope,
            IOutboxService outboxService,
            ILogger<DomainEventsDispatcherService> logger)
        {
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
            this.scope = scope ?? throw new ArgumentNullException(nameof(scope));
            this.outboxService = outboxService ?? throw new ArgumentNullException(nameof(outboxService));
            this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task DispatchAndClearEventsAsync(IEnumerable<IDomainEventsContainer> eventableEntities, CancellationToken cancellationToken = default)
        {
            var domainEvents = eventableEntities.SelectMany(x => x.DomainEvents).ToList();

            var domainEventNotifications = new List<IDomainEventNotification<IDomainEvent>>();
            foreach (var domainEvent in domainEvents)
            {
                // Instanciate the domain notification.
                Type domainEventNotificationType = typeof(IDomainEventNotification<>);
                var domainNotificationWithGenericType = domainEventNotificationType.MakeGenericType(domainEvent.GetType());
                var domainNotification = this.scope.ResolveOptional(domainNotificationWithGenericType, new List<Parameter>
                {
                    new NamedParameter("domainEvent", domainEvent),
                });

                if (domainNotification != null)
                {
                    domainEventNotifications.Add(domainNotification as IDomainEventNotification<IDomainEvent>);
                }
            }

            foreach (var entity in eventableEntities)
            {
                entity.ClearDomainEvents();
            }

            var tasks = domainEvents
                .Select(async (domainEvent) =>
                {
                    try
                    {
                        domainEvent.IsPublished = true;

                        this.logger.LogInformation("Dispatched Domain Event {DomainEvent}", domainEvent.GetType().Name);

                        await this.mediator.Publish(domainEvent, cancellationToken);
                    }
                    catch (Exception exception)
                    {
                        this.logger.LogError(
                           exception,
                           "Error publishing domain event: {@Name} {Content}.",
                           domainEvent.GetType().Name,
                           domainEvent);

                        throw;
                    }
                });

            await Task.WhenAll(tasks);

            foreach (var domainEventNotification in domainEventNotifications)
            {
                await this.outboxService.AddMessageAsync(domainEventNotification, cancellationToken);
            }
        }
    }
}