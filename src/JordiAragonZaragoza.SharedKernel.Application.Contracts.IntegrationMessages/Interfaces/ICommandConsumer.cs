namespace JordiAragonZaragoza.SharedKernel.Application.Contracts.IntegrationMessages.Interfaces
{
    using MassTransit;

    public interface ICommandConsumer<in TCommand> : IConsumer<TCommand>
        where TCommand : class, IIntegrationCommand
    {
    }
}