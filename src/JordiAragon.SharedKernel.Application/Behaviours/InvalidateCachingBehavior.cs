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
        private readonly ILogger<InvalidateCachingBehavior<TRequest, TResponse>> logger;
        private readonly ICacheService cacheService;

        public InvalidateCachingBehavior(
            ICacheService cacheService,
            ILogger<InvalidateCachingBehavior<TRequest, TResponse>> logger)
        {
            this.logger = Guard.Against.Null(logger, nameof(logger));
            this.cacheService = Guard.Against.Null(cacheService, nameof(cacheService));
        }

        public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
        {
            var response = await next();

            var cacheKey = request.PrefixCacheKey;
            await this.cacheService.RemoveByPrefixAsync(cacheKey, cancellationToken);

            this.logger.LogInformation("Cache data with cacheKey: {CacheKey} removed.", cacheKey);

            return response;
        }
    }
}