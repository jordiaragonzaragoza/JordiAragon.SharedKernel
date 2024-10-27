namespace JordiAragonZaragoza.SharedKernel.Application.Contracts.Interfaces
{
    using Ardalis.Result;
    using JordiAragonZaragoza.SharedKernel.Contracts.Events;
    using MediatR;

    public interface ICommandHandler<TCommand, TResponse> : IRequestHandler<TCommand, Result<TResponse>>, IEventsContainer<IApplicationEvent>
        where TCommand : ICommand<TResponse>
        where TResponse : notnull
    {
    }
}