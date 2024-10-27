﻿namespace JordiAragonZaragoza.SharedKernel.Application.Contracts.Interfaces
{
    using Ardalis.Result;
    using MediatR;

    /// <summary>
    /// Command with response operation. To see pure DDD commands check <see cref="ICommand"/>.
    /// </summary>
    /// <typeparam name="TResponse">The response operation.</typeparam>
    public interface ICommand<TResponse> : IRequest<Result<TResponse>>, IBaseCommand
        where TResponse : notnull
    {
    }
}