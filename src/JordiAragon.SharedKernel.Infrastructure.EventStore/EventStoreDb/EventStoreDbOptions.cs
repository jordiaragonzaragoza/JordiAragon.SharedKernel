namespace JordiAragon.SharedKernel.Infrastructure.EventStore.EventStoreDb
{
    public sealed class EventStoreDbOptions
    {
        public const string Section = "EventStore";

        public string ConnectionString { get; set; }
    }
}