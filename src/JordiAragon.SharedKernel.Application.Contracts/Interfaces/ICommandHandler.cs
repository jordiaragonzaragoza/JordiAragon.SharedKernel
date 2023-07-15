namespace JordiAragon.SharedKernel.Application.Contracts.Interfaces
{
    using Ardalis.Result;
    using JordiAragon.SharedKernel.Contracts.Events;
    using MediatR;

    public interface ICommandHandler<TCommand, TResponse> : IRequestHandler<TCommand, Result<TResponse>>, IEventsContainer<IApplicationEvent>
        where TCommand : ICommand<TResponse>
    {
    }
}