namespace JordiAragon.SharedKernel.Application.Contracts.Interfaces
{
    using System.Collections.Generic;
    using Ardalis.Result;
    using MediatR;

    public interface ICommandHandler<TCommand> : IRequestHandler<TCommand, Result>
        where TCommand : ICommand
    {
        IEnumerable<IApplicationEvent> ApplicationEvents { get; }
    }
}