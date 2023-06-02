namespace JordiAragon.SharedKernel.Application.Behaviours
{
    using System.Diagnostics;
    using System.Text.Json;
    using System.Threading;
    using System.Threading.Tasks;
    using Ardalis.GuardClauses;
    using Ardalis.Result;
    using JordiAragon.SharedKernel.Application.Contracts.Interfaces;
    using MediatR;
    using Microsoft.Extensions.Logging;

    public class PerformanceBehaviour<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
        where TRequest : IRequest<TResponse>
        where TResponse : IResult
    {
        private readonly Stopwatch timer;
        private readonly ILogger<TRequest> logger;
        private readonly ICurrentUserService currentUserService;

        public PerformanceBehaviour(
            ILogger<TRequest> logger,
            ICurrentUserService currentUserService)
        {
            this.timer = new Stopwatch();

            this.logger = Guard.Against.Null(logger, nameof(logger));
            this.currentUserService = Guard.Against.Null(currentUserService, nameof(currentUserService));
        }

        public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
        {
            this.timer.Start();

            var response = await next();

            this.timer.Stop();

            var requestName = typeof(TRequest).Name;
            var userId = this.currentUserService.UserId ?? string.Empty;
            var requestSerialized = JsonSerializer.Serialize(request);

            // Get Ardalis.Result.Value or Ardalis.Result<T>.Value property.
            var responseValue = typeof(TResponse).GetProperty("Value").GetValue(response, null);
            var responseSerialized = JsonSerializer.Serialize(responseValue);

            var elapsedMilliseconds = this.timer.ElapsedMilliseconds;

            if (elapsedMilliseconds > 1500)
            {
                this.logger.LogWarning(
                    "Long Running Request: {RequestName} Elapsed Time: {ElapsedMilliseconds} milliseconds User ID: {@UserId} Request Data: {RequestSerialized}",
                    requestName,
                    elapsedMilliseconds,
                    userId,
                    requestSerialized);
            }

            this.logger.LogInformation(
                "Request Completed: {RequestName} Elapsed Time: {ElapsedMilliseconds} milliseconds User ID: {@UserId} Request Data: {RequestSerialized} Response Data: {ResponseSerialized}",
                requestName,
                elapsedMilliseconds,
                userId,
                requestSerialized,
                responseSerialized);

            return response;
        }
    }
}
