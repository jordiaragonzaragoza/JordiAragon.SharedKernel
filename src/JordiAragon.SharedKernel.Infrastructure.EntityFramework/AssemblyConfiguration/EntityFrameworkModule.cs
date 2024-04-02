namespace JordiAragon.SharedKernel.Infrastructure.EntityFramework.AssemblyConfiguration
{
    using System.Reflection;
    using Autofac;
    using JordiAragon.SharedKernel;
    using JordiAragon.SharedKernel.Contracts.Repositories;
    using JordiAragon.SharedKernel.Infrastructure.EntityFramework.Interceptors;
    using JordiAragon.SharedKernel.Infrastructure.EntityFramework.Repositories.DataModel;
    using Volo.Abp.Guids;

    public class EntityFrameworkModule : AssemblyModule
    {
        protected override Assembly CurrentAssembly => InfrastructureEntityFrameworkAssemblyReference.Assembly;

        protected override void Load(ContainerBuilder builder)
        {
            base.Load(builder);

            // Here we can registrate the common EntityFrameworkServices.
            builder.RegisterType(typeof(SequentialGuidGenerator))
                .As(typeof(IGuidGenerator))
                .InstancePerLifetimeScope();

            builder.RegisterType<AuditableEntitySaveChangesInterceptor>()
                .InstancePerLifetimeScope();
        }
    }
}