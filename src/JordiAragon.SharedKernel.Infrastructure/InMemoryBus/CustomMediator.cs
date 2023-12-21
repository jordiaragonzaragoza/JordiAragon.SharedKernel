namespace JordiAragon.SharedKernel.Infrastructure.InMemoryBus
{
    using System;
    using System.Collections.Generic;
    using System.Threading;
    using System.Threading.Tasks;
    using MediatR;
    using MediatR.NotificationPublishers;

    /// <summary>
    /// This class is required due to an issue with Autofac, Decorators and .NET DI.
    /// </summary>
    public class CustomMediator : Mediator
    {
        public CustomMediator(IServiceProvider serviceProvider)
            : base(serviceProvider, new ForeachAwaitPublisher())
        {
        }

        //// Unfortunately Publish() seems to hit the handlers twice!!!
        //// https://github.com/jbogard/MediatR/issues/702
        //// https://github.com/jbogard/MediatR/issues/718
        //// https://github.com/jbogard/MediatR.Extensions.Microsoft.DependencyInjection/issues/118
        protected override async Task PublishCore(
            IEnumerable<NotificationHandlerExecutor> handlerExecutors,
            INotification notification,
            CancellationToken cancellationToken)
        {
            var newHandlerExecutors = new List<NotificationHandlerExecutor>();
            foreach (var handler in handlerExecutors)
            {
                if (!newHandlerExecutors.Exists(n => n.HandlerInstance.GetType() == handler.HandlerInstance.GetType()))
                {
                    newHandlerExecutors.Add(handler);
                }
            }

            await base.PublishCore(newHandlerExecutors, notification, cancellationToken);
        }
    }
}