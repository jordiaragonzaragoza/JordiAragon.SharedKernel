namespace JordiAragon.SharedKernel.Infrastructure.Cache.EasyCaching
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;
    using global::EasyCaching.Core;
    using JordiAragon.SharedKernel.Application.Contracts.Interfaces;
    using JordiAragon.SharedKernel.Contracts.DependencyInjection;
    using Microsoft.Extensions.Options;

    public class EasyCachingCacheService : ICacheService, ISingletonDependency
    {
        private readonly CacheOptions easyCachingOptions;
        private readonly IEasyCachingProvider cachingProvider;

        public EasyCachingCacheService(
            IEasyCachingProviderFactory cachingFactory,
            IOptions<CacheOptions> easyCachingOptions)
        {
            this.easyCachingOptions = easyCachingOptions.Value ?? throw new ArgumentNullException(nameof(easyCachingOptions));
            this.cachingProvider = cachingFactory.GetCachingProvider(this.easyCachingOptions.DefaultName);
        }

        public async Task<ICacheValue<T>> GetAsync<T>(string cacheKey, CancellationToken cancellationToken = default)
        {
            return new EasyCachingCacheValueAdapter<T>(await this.cachingProvider.GetAsync<T>(cacheKey, cancellationToken));
        }

        public async Task RemoveByPrefixAsync(string prefix, CancellationToken cancellationToken = default)
        {
            await this.cachingProvider.RemoveByPrefixAsync(prefix, cancellationToken);
        }

        public async Task SetAsync<T>(string cacheKey, T cacheValue, TimeSpan? expiration = null, CancellationToken cancellationToken = default)
        {
            var expirationTime = expiration ?? TimeSpan.FromSeconds(this.easyCachingOptions.DefaultExpirationInSeconds);

            await this.cachingProvider.SetAsync(cacheKey, cacheValue, expirationTime, cancellationToken);
        }
    }
}