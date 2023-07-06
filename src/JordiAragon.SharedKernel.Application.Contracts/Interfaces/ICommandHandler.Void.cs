namespace JordiAragon.SharedKernel.Application.Contracts.Interfaces
{
    using Ardalis.Result;
    using MediatR;

    public interface ICommandHandler<TCommand> : IRequestHandler<TCommand, Result>, IApplicationEventsContainer
        where TCommand : ICommand
    {
    }
}