namespace JordiAragonZaragoza.SharedKernel.Application.Contracts.Interfaces
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;

    public interface ICacheService
    {
        Task SetAsync<T>(string cacheKey, T cacheValue, TimeSpan? expiration = null, CancellationToken cancellationToken = default);

        Task<ICacheValue<T>> GetAsync<T>(string cacheKey, CancellationToken cancellationToken = default);

        Task RemoveByPrefixAsync(string prefix, CancellationToken cancellationToken = default);
    }
}