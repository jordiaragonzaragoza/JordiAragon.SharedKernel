namespace JordiAragonZaragoza.SharedKernel.Application.Contracts.Interfaces
{
    using Ardalis.Result;
    using JordiAragonZaragoza.SharedKernel.Contracts.Events;
    using MediatR;

    public interface ICommandHandler<TCommand> : IRequestHandler<TCommand, Result>, IEventsContainer<IApplicationEvent>
        where TCommand : ICommand
    {
    }
}