namespace JordiAragon.SharedKernel.Infrastructure.EventStore.AssemblyConfiguration
{
    using System.Reflection;
    using JordiAragon.SharedKernel;

    public sealed class EventStoreModule : AssemblyModule
    {
        protected override Assembly CurrentAssembly => InfrastructureEventStoreAssemblyReference.Assembly;
    }
}