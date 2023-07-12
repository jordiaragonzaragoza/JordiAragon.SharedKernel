namespace JordiAragon.SharedKernel.Application.Events.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;
    using Autofac;
    using Autofac.Core;
    using JordiAragon.SharedKernel.Application.Contracts.Interfaces;
    using JordiAragon.SharedKernel.Contracts.Outbox;
    using MediatR;
    using Microsoft.Extensions.Logging;

    public class ApplicationEventsDispatcherService : IApplicationEventsDispatcherService
    {
        private readonly IPublisher mediator;
        private readonly ILifetimeScope scope;
        private readonly IOutboxService outboxService;
        private readonly ILogger<ApplicationEventsDispatcherService> logger;

        public ApplicationEventsDispatcherService(
            IPublisher mediator,
            ILifetimeScope scope,
            IOutboxService outboxService,
            ILogger<ApplicationEventsDispatcherService> logger)
        {
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
            this.scope = scope ?? throw new ArgumentNullException(nameof(scope));
            this.outboxService = outboxService ?? throw new ArgumentNullException(nameof(outboxService));
            this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task DispatchEventsAsync(IEnumerable<IApplicationEvent> applicationEvents, CancellationToken cancellationToken = default)
        {
            var applicationEventsList = applicationEvents.ToList();

            var applicationEventNotifications = new List<IApplicationEventNotification<IApplicationEvent>>();
            foreach (var applicationEvent in applicationEventsList)
            {
                // Instanciate the application notification.
                Type applicationEvenNotificationType = typeof(IApplicationEventNotification<>);
                var applicationNotificationWithGenericType = applicationEvenNotificationType.MakeGenericType(applicationEvent.GetType());
                var applicationNotification = this.scope.ResolveOptional(applicationNotificationWithGenericType, new List<Parameter>
                {
                    new NamedParameter("event", applicationEvent),
                });

                if (applicationNotification != null)
                {
                    applicationEventNotifications.Add(applicationNotification as IApplicationEventNotification<IApplicationEvent>);
                }
            }

            foreach (var applicationEvent in applicationEventsList)
            {
                try
                {
                    applicationEvent.IsPublished = true;

                    this.logger.LogInformation("Dispatched Application Event {DomainEvent}", applicationEvent.GetType().Name);

                    await this.mediator.Publish(applicationEvent, cancellationToken);
                }
                catch (Exception exception)
                {
                    this.logger.LogError(
                       exception,
                       "Error publishing application event: {@Name} {Content}.",
                       applicationEvent.GetType().Name,
                       applicationEvent);

                    throw;
                }
            }

            foreach (var applicationEventNotification in applicationEventNotifications)
            {
                await this.outboxService.AddMessageAsync(applicationEventNotification, cancellationToken);
            }
        }
    }
}