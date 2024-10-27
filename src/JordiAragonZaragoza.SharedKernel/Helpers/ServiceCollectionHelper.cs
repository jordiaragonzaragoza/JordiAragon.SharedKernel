namespace JordiAragonZaragoza.SharedKernel.Helpers
{
    using System.Linq;
    using Ardalis.GuardClauses;
    using Microsoft.Extensions.DependencyInjection;

    public static class ServiceCollectionHelper
    {
        public static IServiceCollection Remove<TService>(this IServiceCollection services)
        {
            Guard.Against.Null(services, nameof(services));

            var serviceDescriptor = services.FirstOrDefault(d =>
                d.ServiceType == typeof(TService));

            if (serviceDescriptor != null)
            {
                services.Remove(serviceDescriptor);
            }

            return services;
        }
    }
}