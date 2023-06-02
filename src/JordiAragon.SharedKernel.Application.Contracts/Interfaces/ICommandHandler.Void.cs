namespace JordiAragon.SharedKernel.Application.Contracts.Interfaces
{
    using System;
    using Ardalis.Result;
    using MediatR;

    public interface ICommandHandler<TCommand> : IRequestHandler<TCommand, Result>
        where TCommand : ICommand
    {
    }
}