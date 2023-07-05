namespace JordiAragon.SharedKernel.Application.Contracts.Interfaces
{
    using System.Collections.Generic;
    using Ardalis.Result;
    using MediatR;

    public interface ICommandHandler<TCommand, TResponse> : IRequestHandler<TCommand, Result<TResponse>>
        where TCommand : ICommand<TResponse>
    {
        IEnumerable<IApplicationEvent> ApplicationEvents { get; }
    }
}