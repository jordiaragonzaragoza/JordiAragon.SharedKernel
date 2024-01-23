namespace JordiAragon.SharedKernel.Application.Behaviours
{
    using System.Text.Json;
    using System.Threading;
    using System.Threading.Tasks;
    using Ardalis.GuardClauses;
    using JordiAragon.SharedKernel.Application.Contracts.Interfaces;
    using MediatR.Pipeline;
    using Microsoft.Extensions.Logging;

    /// <summary>
    /// Defined a request pre-processor for a handler.
    /// </summary>
    /// <typeparam name="TRequest">Request type.</typeparam>
    public class LoggerBehaviour<TRequest> : IRequestPreProcessor<TRequest>
        where TRequest : notnull
    {
        private readonly ILogger<TRequest> logger;
        private readonly ICurrentUserService currentUserService;

        public LoggerBehaviour(
            ILogger<TRequest> logger,
            ICurrentUserService currentUserService)
        {
            this.logger = Guard.Against.Null(logger, nameof(logger));
            this.currentUserService = Guard.Against.Null(currentUserService, nameof(currentUserService));
        }

        /// <summary>
        /// Process method executes before calling the Handle method on your handler.
        /// </summary>
        /// <param name="request">Incoming request.</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        /// <returns>An awaitable task.</returns>
        public Task Process(TRequest request, CancellationToken cancellationToken)
        {
            var requestName = typeof(TRequest).Name;
            var userId = this.currentUserService.UserId ?? string.Empty;
            var requestSerialized = JsonSerializer.Serialize(request);

            this.logger.LogInformation("Request: {RequestName} User ID: {@UserId} Request Data: {RequestSerialized}", requestName, userId, requestSerialized);

            return Task.CompletedTask;
        }
    }
}

/*
 namespace JordiAragon.SharedKernel.Application.Behaviours
{
    using System.Text.Json;
    using System.Threading;
    using System.Threading.Tasks;
    using JordiAragon.SharedKernel.Application.Contracts.Interfaces;
    using MediatR.Pipeline;
    using Microsoft.Extensions.Logging;

    /// <summary>
    /// Defined a request pre-processor for a handler.
    /// </summary>
    /// <typeparam name="TRequest">Request type.</typeparam>
    public class LoggerBehaviour<TRequest> : IRequestPreProcessor<TRequest>
        where TRequest : notnull
    {
        private readonly ILogger<TRequest> logger;
        private readonly ICurrentUserService currentUserService;
        private readonly IIdentityService identityService;

        public LoggerBehaviour(
            ILogger<TRequest> logger,
            ICurrentUserService currentUserService,
            IIdentityService identityService)
        {
            this.logger = logger;
            this.currentUserService = currentUserService;
            this.identityService = identityService;
        }

        /// <summary>
        /// Process method executes before calling the Handle method on your handler.
        /// </summary>
        /// <param name="request">Incoming request.</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        /// <returns>An awaitable task.</returns>
        public async Task Process(TRequest request, CancellationToken cancellationToken)
        {
            var requestName = typeof(TRequest).Name;
            var userId = this.currentUserService.UserId ?? string.Empty;
            var requestSerialized = JsonSerializer.Serialize(request);
            var userName = "Anonymous";

            if (!string.IsNullOrEmpty(userId))
            {
                var response = await this.identityService.GetUserNameAsync(userId);
                if (response.IsSuccess)
                {
                    userName = response.Value;
                }
            }

            this.logger.LogInformation("Request: {RequestName} User: {@UserId} {@UserName} Request Data: {RequestSerialized}", requestName, userId, userName, requestSerialized);
        }
    }
}
 */