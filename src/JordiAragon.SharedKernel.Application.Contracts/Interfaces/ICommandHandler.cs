﻿namespace JordiAragon.SharedKernel.Application.Contracts.Interfaces
{
    using System;
    using Ardalis.Result;
    using MediatR;

    public interface ICommandHandler<TCommand, TResponse> : IRequestHandler<TCommand, Result<TResponse>>
        where TCommand : ICommand<TResponse>
    {
    }
}