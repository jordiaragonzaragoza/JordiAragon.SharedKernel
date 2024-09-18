namespace JordiAragon.SharedKernel.Application.Behaviours
{
    using System.Threading;
    using System.Threading.Tasks;
    using Ardalis.GuardClauses;
    using Ardalis.Result;
    using JordiAragon.SharedKernel.Application.Contracts.Interfaces;
    using MediatR;
    using Microsoft.Extensions.Logging;

    public class InvalidateCachingBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
        where TRequest : IInvalidateCacheRequest
        where TResponse : IResult
    {
        private readonly ILogger<TRequest> logger;
        private readonly ICacheService cacheService;

        public InvalidateCachingBehavior(
            ICacheService cacheService,
            ILogger<TRequest> logger)
        {
            this.logger = Guard.Against.Null(logger, nameof(logger));
            this.cacheService = Guard.Against.Null(cacheService, nameof(cacheService));
        }

        public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
        {
            Guard.Against.Null(next, nameof(next));

            var response = await next();

            await this.cacheService.RemoveByPrefixAsync(request.PrefixCacheKey, cancellationToken);

            this.logger.LogInformation("Cache data with cacheKey: {CacheKey} removed.", request.PrefixCacheKey);

            return response;
        }
    }
}