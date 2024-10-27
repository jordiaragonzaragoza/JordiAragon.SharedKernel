namespace JordiAragonZaragoza.SharedKernel.Infrastructure.EventStore.AssemblyConfiguration
{
    using System.Reflection;
    using JordiAragonZaragoza.SharedKernel;

    public sealed class EventStoreModule : AssemblyModule
    {
        protected override Assembly CurrentAssembly => InfrastructureEventStoreAssemblyReference.Assembly;
    }
}