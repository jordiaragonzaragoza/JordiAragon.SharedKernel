namespace JordiAragon.SharedKernel.Application.Contracts.Interfaces
{
    using Ardalis.Result;
    using JordiAragon.SharedKernel.Contracts.Events;
    using MediatR;

    public interface ICommandHandler<TCommand> : IRequestHandler<TCommand, Result>, IEventsContainer<IApplicationEvent>
        where TCommand : ICommand
    {
    }
}