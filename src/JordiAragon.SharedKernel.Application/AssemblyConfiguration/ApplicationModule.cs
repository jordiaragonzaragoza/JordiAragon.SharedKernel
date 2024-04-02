namespace JordiAragon.SharedKernel.Application.AssemblyConfiguration
{
    using System.Reflection;
    using Autofac;
    using JordiAragon.SharedKernel;
    using JordiAragon.SharedKernel.Application.Commands.Decorators;
    using JordiAragon.SharedKernel.Application.Events.Decorators;
    using MediatR;

    public class ApplicationModule : AssemblyModule
    {
        protected override Assembly CurrentAssembly => ApplicationAssemblyReference.Assembly;

        protected override void Load(ContainerBuilder builder)
        {
            base.Load(builder);

            builder.RegisterGenericDecorator(
                typeof(DomainEventsHandlerDecorator<>),
                typeof(INotificationHandler<>));

            builder.RegisterGenericDecorator(
                typeof(EventNotificationHandlerDecorator<>),
                typeof(INotificationHandler<>));

            builder.RegisterGenericDecorator(
                typeof(CommandHandlerDecorator<>),
                typeof(IRequestHandler<,>));

            builder.RegisterGenericDecorator(
                typeof(CommandHandlerDecorator<,>),
                typeof(IRequestHandler<,>));
        }
    }
}