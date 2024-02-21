﻿namespace JordiAragon.SharedKernel.Infrastructure.AssemblyConfiguration
{
    using System.Reflection;
    using Autofac;
    using JordiAragon.SharedKernel;
    using JordiAragon.SharedKernel.Infrastructure.InternalBus;
    using MediatR;

    public class InfrastructureModule : AssemblyModule
    {
        protected override Assembly CurrentAssembly => InfrastructureAssemblyReference.Assembly;

        protected override void Load(ContainerBuilder builder)
        {
            base.Load(builder);

            builder.RegisterType(typeof(CustomMediator))
                .As(typeof(IMediator))
                .InstancePerLifetimeScope();
        }
    }
}