namespace JordiAragonZaragoza.SharedKernel
{
    using System.Reflection;
    using Autofac;
    using JordiAragonZaragoza.SharedKernel.Contracts.DependencyInjection;
    using Module = Autofac.Module;

    public abstract class AssemblyModule : Module
    {
        protected abstract Assembly CurrentAssembly { get; }

        protected override void Load(ContainerBuilder builder)
        {
            // Register Transient dependencies.
            builder.RegisterAssemblyTypes(this.CurrentAssembly)
                .Where(x => typeof(ITransientDependency).IsAssignableFrom(x))
                .AsImplementedInterfaces()
                .InstancePerDependency();

            // Register Scoped dependencies.
            builder.RegisterAssemblyTypes(this.CurrentAssembly)
                .Where(x => typeof(IScopedDependency).IsAssignableFrom(x))
                .AsImplementedInterfaces()
                .InstancePerLifetimeScope();

            // Register singleton dependencies.
            builder.RegisterAssemblyTypes(this.CurrentAssembly)
                .Where(x => typeof(ISingletonDependency).IsAssignableFrom(x))
                .AsImplementedInterfaces()
                .SingleInstance();

            // Default without marker interface, register as transient.
            builder.RegisterAssemblyTypes(this.CurrentAssembly)
               .Where(x =>
                    !typeof(ITransientDependency).IsAssignableFrom(x)
                    && !typeof(IScopedDependency).IsAssignableFrom(x)
                    && !typeof(ISingletonDependency).IsAssignableFrom(x)
                    && !typeof(IIgnoreDependency).IsAssignableFrom(x))
               .AsImplementedInterfaces()
               .InstancePerDependency();
        }
    }
}