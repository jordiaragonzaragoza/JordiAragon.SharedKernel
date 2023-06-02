namespace JordiAragon.SharedKernel.Helpers
{
    using System.Linq;
    using Microsoft.Extensions.DependencyInjection;

    public static class ServiceCollectionHelper
    {
        public static IServiceCollection Remove<TService>(this IServiceCollection services)
        {
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