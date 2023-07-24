namespace JordiAragon.SharedKernel.Infrastructure.EventStore.AssemblyConfiguration
{
    using System.Reflection;
    using JordiAragon.SharedKernel;

    public class EventStoreModule : AssemblyModule
    {
        protected override Assembly CurrentAssembly => InfrastructureEventStoreAssemblyReference.Assembly;
    }
}