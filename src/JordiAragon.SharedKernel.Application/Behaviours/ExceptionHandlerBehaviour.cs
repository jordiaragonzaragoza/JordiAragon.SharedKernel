﻿namespace JordiAragon.SharedKernel.Application.Behaviours
{
    using System;
    using System.Text.Json;
    using System.Threading;
    using System.Threading.Tasks;
    using Ardalis.Result;
    using JordiAragon.SharedKernel.Application.Contracts.Interfaces;
    using MediatR;
    using Microsoft.Extensions.Logging;

    public class ExceptionHandlerBehaviour<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
        where TRequest : IRequest<TResponse>
        where TResponse : IResult
    {
        private readonly ILogger<TRequest> logger;
        private readonly ICurrentUserService currentUserService;

        public ExceptionHandlerBehaviour(
            ILogger<TRequest> logger,
            ICurrentUserService currentUserService)
        {
            this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
            this.currentUserService = currentUserService ?? throw new ArgumentNullException(nameof(currentUserService));
        }

        public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
        {
            try
            {
                return await next();
            }
            catch (Exception exception)
            {
                var requestName = typeof(TRequest).Name;
                var userId = this.currentUserService.UserId ?? string.Empty;
                var requestSerialized = JsonSerializer.Serialize(request);

                this.logger.LogError(
                    exception,
                    "Unhandled Exception. Request: {RequestName}  User ID: {@UserId} Request Data: {RequestSerialized}",
                    requestName,
                    userId,
                    requestSerialized);

                throw;
            }
        }
    }
}
