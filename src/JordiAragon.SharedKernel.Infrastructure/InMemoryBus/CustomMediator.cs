namespace JordiAragon.SharedKernel.Infrastructure.InMemoryBus
{
    using System;
    using MediatR;

    /// <summary>
    /// This class is required due to an issue with Autofac, Decorators and .NET DI.
    /// </summary>
    public class CustomMediator : Mediator
    {
        public CustomMediator(IServiceProvider serviceProvider)
            : base(serviceProvider, new ForeachAwaitPublisher())
        {
        }
    }
}