namespace JordiAragon.SharedKernel.Application.Contracts.Interfaces
{
    using System;

    public interface ICacheRequest
    {
        string CacheKey { get; } // TODO: Review GetType().FullName or Aggregate

        TimeSpan? AbsoluteExpirationInSeconds { get; }
    }
}