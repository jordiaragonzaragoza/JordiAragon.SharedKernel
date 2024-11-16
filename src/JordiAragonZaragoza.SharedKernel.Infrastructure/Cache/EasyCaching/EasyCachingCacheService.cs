namespace JordiAragonZaragoza.SharedKernel.Infrastructure.Cache.EasyCaching
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;
    using global::EasyCaching.Core;
    using JordiAragonZaragoza.SharedKernel.Application.Contracts.Interfaces;
    using JordiAragonZaragoza.SharedKernel.Contracts.DependencyInjection;
    using Microsoft.Extensions.Options;

    public class EasyCachingCacheService : ICacheService, ISingletonDependency
    {
        private readonly CacheOptions easyCachingOptions;
        private readonly IEasyCachingProvider cachingProvider;

        public EasyCachingCacheService(
            IEasyCachingProviderFactory cachingFactory,
            IOptions<CacheOptions> easyCachingOptions)
        {
            ArgumentNullException.ThrowIfNull(cachingFactory);
            ArgumentNullException.ThrowIfNull(easyCachingOptions);

            this.easyCachingOptions = easyCachingOptions.Value;
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