namespace JordiAragonZaragoza.SharedKernel.Infrastructure.Cache.EasyCaching
{
    using global::EasyCaching.Core;
    using JordiAragonZaragoza.SharedKernel.Application.Contracts.Interfaces;

    public class EasyCachingCacheValueAdapter<T> : ICacheValue<T>
    {
        private readonly CacheValue<T> cacheValue;

        public EasyCachingCacheValueAdapter(CacheValue<T> cacheValue)
        {
            this.cacheValue = cacheValue;
        }

        public bool HasValue => this.cacheValue.HasValue;

        public bool IsNull => this.cacheValue.IsNull;

        public T Value => this.cacheValue.Value;
    }
}