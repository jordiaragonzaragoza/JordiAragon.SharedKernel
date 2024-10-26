namespace JordiAragonZaragoza.SharedKernel.Application.Contracts.IntegrationMessages.Interfaces
{
    using MassTransit;

    public interface IEventConsumer<in TEvent> : IConsumer<TEvent>
        where TEvent : class, IIntegrationEvent
    {
    }
}