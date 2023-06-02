namespace JordiAragon.SharedKernel.Application.Contracts.IntegrationMessages.Interfaces
{
    using MassTransit;
    using static MassTransit.Monitoring.Performance.BuiltInCounters;

    public interface IEventConsumer<in TEvent> : IConsumer<TEvent>
        where TEvent : class, IIntegrationEvent
    {
    }
}