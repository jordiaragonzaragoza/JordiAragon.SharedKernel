namespace JordiAragon.SharedKernel.Application.AssemblyConfiguration
{
    using JordiAragon.SharedKernel.Application.Behaviours;
    using MediatR;
    using MediatR.Pipeline;
    using Microsoft.Extensions.DependencyInjection;

    public static class ConfigureServices
    {
        public static IServiceCollection AddSharedKernelApplicationServices(this IServiceCollection serviceCollection)
        {
            // Register pipeline behaviors for cross cutting concerns.
            serviceCollection.AddTransient(typeof(IRequestPreProcessor<>), typeof(LoggerBehaviour<>));
            serviceCollection.AddTransient(typeof(IPipelineBehavior<,>), typeof(ExceptionHandlerBehaviour<,>));
            serviceCollection.AddTransient(typeof(IPipelineBehavior<,>), typeof(UnitOfWorkBehaviour<,>));
            serviceCollection.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehaviour<,>));
            serviceCollection.AddTransient(typeof(IPipelineBehavior<,>), typeof(CachingBehavior<,>));
            serviceCollection.AddTransient(typeof(IPipelineBehavior<,>), typeof(InvalidateCachingBehavior<,>));
            serviceCollection.AddTransient(typeof(IPipelineBehavior<,>), typeof(DomainEventsDispatcherBehaviour<,>));
            serviceCollection.AddTransient(typeof(IPipelineBehavior<,>), typeof(PerformanceBehaviour<,>));

            return serviceCollection;
        }
    }
}