﻿namespace JordiAragon.SharedKernel.Infrastructure.InternalBus
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;
    using Ardalis.Result;
    using global::MediatR;
    using JordiAragon.SharedKernel.Application.Contracts.Interfaces;

    public class QueryBus : IQueryBus
    {
        private readonly ISender sender;

        public QueryBus(ISender sender)
        {
            this.sender = sender ?? throw new ArgumentNullException(nameof(sender));
        }

        public Task<Result<TResponse>> SendAsync<TResponse>(IQuery<TResponse> query, CancellationToken cancellationToken = default)
            => this.sender.Send(query, cancellationToken);
    }
}