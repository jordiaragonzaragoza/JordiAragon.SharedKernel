namespace JordiAragon.SharedKernel.Infrastructure.EntityFramework
{
    using System;
    using System.Collections.Generic;
    using System.Threading;
    using System.Threading.Tasks;
    using Ardalis.GuardClauses;
    using Ardalis.Result;
    using Ardalis.Specification;
    using Ardalis.Specification.EntityFrameworkCore;
    using JordiAragon.SharedKernel.Application.Contracts.Interfaces;
    using JordiAragon.SharedKernel.Contracts.DependencyInjection;
    using JordiAragon.SharedKernel.Domain.Contracts.Interfaces;
    using Microsoft.Extensions.Logging;

    public abstract class BaseCachedRepository<T> : RepositoryBase<T>, ICachedRepository<T>, IScopedDependency
        where T : class, IAggregateRoot
    {
        private readonly ICacheService cacheService;
        private readonly ILogger<BaseCachedRepository<T>> logger;

        protected BaseCachedRepository(
            BaseContext dbContext,
            ILogger<BaseCachedRepository<T>> logger,
            ICacheService cacheService)
            : base(dbContext)
        {
            this.cacheService = Guard.Against.Null(cacheService, nameof(cacheService));
            this.logger = Guard.Against.Null(logger, nameof(logger));
        }

        public string CacheKey => $"{typeof(T)}";

        public override async Task<T> AddAsync(T entity, CancellationToken cancellationToken = default)
        {
            var response = await base.AddAsync(entity, cancellationToken);

            await this.CacheServiceRemoveByPrefixAsync(cancellationToken);

            return response;
        }

        public override async Task<IEnumerable<T>> AddRangeAsync(IEnumerable<T> entities, CancellationToken cancellationToken = default)
        {
            var response = await base.AddRangeAsync(entities, cancellationToken);

            await this.CacheServiceRemoveByPrefixAsync(cancellationToken);

            return response;
        }

        public override async Task UpdateAsync(T entity, CancellationToken cancellationToken = default)
        {
            await base.UpdateAsync(entity, cancellationToken);

            await this.CacheServiceRemoveByPrefixAsync(cancellationToken);
        }

        public override async Task UpdateRangeAsync(IEnumerable<T> entities, CancellationToken cancellationToken = default)
        {
            await base.UpdateRangeAsync(entities, cancellationToken);

            await this.CacheServiceRemoveByPrefixAsync(cancellationToken);
        }

        public override async Task DeleteAsync(T entity, CancellationToken cancellationToken = default)
        {
            await base.DeleteAsync(entity, cancellationToken);

            await this.CacheServiceRemoveByPrefixAsync(cancellationToken);
        }

        public override async Task DeleteRangeAsync(IEnumerable<T> entities, CancellationToken cancellationToken = default)
        {
            await base.DeleteRangeAsync(entities, cancellationToken);

            await this.CacheServiceRemoveByPrefixAsync(cancellationToken);
        }

        public override async Task<T> GetByIdAsync<TId>(TId id, CancellationToken cancellationToken = default)
        {
            var cacheKeyId = $"{this.CacheKey}_{id}";

            var cachedResponse = await this.CacheGetAsync<T>(cacheKeyId, cancellationToken);
            if (cachedResponse.IsSuccess)
            {
                return cachedResponse;
            }

            var response = await base.GetByIdAsync(id, cancellationToken);

            await this.CacheSetAsync(cacheKeyId, response, cancellationToken);

            return response;
        }

        [Obsolete]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Major Code Smell", "S1123:\"Obsolete\" attributes should include explanations", Justification = "This method is added to keep support to base library")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Major Code Smell", "S1133:\"Obsolete\" attributes should include explanations", Justification = "This method is added to keep support to base library")]
        public override async Task<T> GetBySpecAsync(ISpecification<T> specification, CancellationToken cancellationToken = default)
        {
            var cacheKeySpecification = $"{this.CacheKey}_{specification.GetType().FullName}";
            var cachedResponse = await this.CacheGetAsync<T>(cacheKeySpecification, cancellationToken);
            if (cachedResponse.IsSuccess)
            {
                return cachedResponse;
            }

            var response = await base.GetBySpecAsync(specification, cancellationToken);

            await this.CacheSetAsync(cacheKeySpecification, response, cancellationToken);

            return response;
        }

        [Obsolete]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Major Code Smell", "S1123:\"Obsolete\" attributes should include explanations", Justification = "This method is added to keep support to base library")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Major Code Smell", "S1133:\"Obsolete\" attributes should include explanations", Justification = "This method is added to keep support to base library")]
        public override async Task<TResult> GetBySpecAsync<TResult>(ISpecification<T, TResult> specification, CancellationToken cancellationToken = default)
        {
            var cacheKeySpecification = $"{this.CacheKey}_{specification.GetType().FullName}";
            var cachedResponse = await this.CacheGetResultAsync<TResult>(cacheKeySpecification, cancellationToken);
            if (cachedResponse.IsSuccess)
            {
                return cachedResponse;
            }

            var response = await base.GetBySpecAsync(specification, cancellationToken);

            await this.CacheSetResultAsync(cacheKeySpecification, response, cancellationToken);

            return response;
        }

        public override async Task<T> FirstOrDefaultAsync(ISpecification<T> specification, CancellationToken cancellationToken = default)
        {
            var cacheKeySpecification = $"{this.CacheKey}_{specification.GetType().FullName}";

            var cachedResponse = await this.CacheGetAsync<T>(cacheKeySpecification, cancellationToken);
            if (cachedResponse.IsSuccess)
            {
                return cachedResponse;
            }

            var response = await base.FirstOrDefaultAsync(specification, cancellationToken);

            await this.CacheSetAsync(cacheKeySpecification, response, cancellationToken);

            return response;
        }

        public override async Task<TResult> FirstOrDefaultAsync<TResult>(ISpecification<T, TResult> specification, CancellationToken cancellationToken = default)
        {
            var cacheKeySpecification = $"{this.CacheKey}_{specification.GetType().FullName}";
            var cachedResponse = await this.CacheGetResultAsync<TResult>(cacheKeySpecification, cancellationToken);
            if (cachedResponse.IsSuccess)
            {
                return cachedResponse;
            }

            var response = await base.FirstOrDefaultAsync(specification, cancellationToken);

            await this.CacheSetResultAsync(cacheKeySpecification, response, cancellationToken);

            return response;
        }

        public override async Task<T> SingleOrDefaultAsync(ISingleResultSpecification<T> specification, CancellationToken cancellationToken = default)
        {
            var cacheKeySpecification = $"{this.CacheKey}_{specification.GetType().FullName}";
            var cachedResponse = await this.CacheGetAsync<T>(cacheKeySpecification, cancellationToken);
            if (cachedResponse.IsSuccess)
            {
                return cachedResponse;
            }

            var response = await base.SingleOrDefaultAsync(specification, cancellationToken);

            await this.CacheSetAsync(cacheKeySpecification, response, cancellationToken);

            return response;
        }

        public override async Task<TResult> SingleOrDefaultAsync<TResult>(ISingleResultSpecification<T, TResult> specification, CancellationToken cancellationToken = default)
        {
            var cacheKeySpecification = $"{this.CacheKey}_{specification.GetType().FullName}";
            var cachedResponse = await this.CacheGetResultAsync<TResult>(cacheKeySpecification, cancellationToken);
            if (cachedResponse.IsSuccess)
            {
                return cachedResponse;
            }

            var response = await base.SingleOrDefaultAsync(specification, cancellationToken);

            await this.CacheSetResultAsync(cacheKeySpecification, response, cancellationToken);

            return response;
        }

        public override async Task<List<T>> ListAsync(CancellationToken cancellationToken = default)
        {
            var cachedResponse = await this.CacheGetListAsync(this.CacheKey, cancellationToken);
            if (cachedResponse.IsSuccess)
            {
                return cachedResponse;
            }

            var response = await base.ListAsync(cancellationToken);

            await this.CacheSetListAsync(this.CacheKey, response, cancellationToken);

            return response;
        }

        public override async Task<List<T>> ListAsync(ISpecification<T> specification, CancellationToken cancellationToken = default)
        {
            var cacheKeySpecification = $"{this.CacheKey}_{specification.GetType().FullName}";

            var cachedResponse = await this.CacheGetListAsync(cacheKeySpecification, cancellationToken);
            if (cachedResponse.IsSuccess)
            {
                return cachedResponse;
            }

            var response = await base.ListAsync(specification, cancellationToken);

            await this.CacheSetListAsync(cacheKeySpecification, response, cancellationToken);

            return response;
        }

        public override async Task<List<TResult>> ListAsync<TResult>(ISpecification<T, TResult> specification, CancellationToken cancellationToken = default)
        {
            var cacheKeySpecification = $"{this.CacheKey}_{specification.GetType().FullName}";
            var cachedResponse = await this.CacheGetListResultAsync<TResult>(cacheKeySpecification, cancellationToken);
            if (cachedResponse.IsSuccess)
            {
                return cachedResponse;
            }

            var response = await base.ListAsync(specification, cancellationToken);

            await this.CacheSetListResultAsync(cacheKeySpecification, response, cancellationToken);

            return response;
        }

        public override async Task<int> CountAsync(ISpecification<T> specification, CancellationToken cancellationToken = default)
        {
            var cacheKeySpecification = $"{this.CacheKey}_{specification.GetType().FullName}";

            var cachedResponse = await this.CacheGetAsync<int>(cacheKeySpecification, cancellationToken);
            if (cachedResponse.IsSuccess)
            {
                return cachedResponse;
            }

            var response = await base.CountAsync(specification, cancellationToken);

            await this.CacheSetAsync(cacheKeySpecification, response, cancellationToken);

            return response;
        }

        public override async Task<int> CountAsync(CancellationToken cancellationToken = default)
        {
            var cachedResponse = await this.CacheGetAsync<int>(this.CacheKey, cancellationToken);
            if (cachedResponse.IsSuccess)
            {
                return cachedResponse;
            }

            var response = await base.CountAsync(cancellationToken);

            await this.CacheSetAsync(this.CacheKey, response, cancellationToken);

            return response;
        }

        public override async Task<bool> AnyAsync(ISpecification<T> specification, CancellationToken cancellationToken = default)
        {
            var cacheKeySpecification = $"{this.CacheKey}_{specification.GetType().FullName}";
            var cachedResponse = await this.CacheGetAsync<bool>(cacheKeySpecification, cancellationToken);
            if (cachedResponse.IsSuccess)
            {
                return cachedResponse;
            }

            var response = await base.AnyAsync(specification, cancellationToken);

            await this.CacheSetAsync(cacheKeySpecification, response, cancellationToken);

            return response;
        }

        public override async Task<bool> AnyAsync(CancellationToken cancellationToken = default)
        {
            var cachedResponse = await this.CacheGetAsync<bool>(this.CacheKey, cancellationToken);
            if (cachedResponse.IsSuccess)
            {
                return cachedResponse;
            }

            var response = await base.AnyAsync(cancellationToken);

            await this.CacheSetAsync(this.CacheKey, response, cancellationToken);

            return response;
        }

        private async Task<Result<TIn>> CacheGetAsync<TIn>(string cacheKey, CancellationToken cancellationToken)
        {
            var cachedResponse = await this.cacheService.GetAsync<TIn>(cacheKey, cancellationToken);
            if (!cachedResponse.IsNull && cachedResponse.HasValue)
            {
                this.logger.LogInformation("Fetch data from cache with cacheKey: {cacheKey}", cacheKey);

                return Result.Success(cachedResponse.Value);
            }

            return Result.NotFound();
        }

        private async Task<Result<List<T>>> CacheGetListAsync(string cacheKey, CancellationToken cancellationToken)
        {
            var cachedResponse = await this.cacheService.GetAsync<List<T>>(cacheKey, cancellationToken);
            if (!cachedResponse.IsNull && cachedResponse.HasValue)
            {
                this.logger.LogInformation("Fetch data from cache with cacheKey: {cacheKey}", cacheKey);

                return Result.Success(cachedResponse.Value);
            }

            return Result.NotFound();
        }

        private async Task<Result<TResult>> CacheGetResultAsync<TResult>(string cacheKey, CancellationToken cancellationToken)
        {
            var cachedResponse = await this.cacheService.GetAsync<TResult>(cacheKey, cancellationToken);
            if (!cachedResponse.IsNull && cachedResponse.HasValue)
            {
                this.logger.LogInformation("Fetch data from cache with cacheKey: {cacheKey}", cacheKey);

                return Result.Success(cachedResponse.Value);
            }

            return Result.NotFound();
        }

        private async Task<Result<List<TResult>>> CacheGetListResultAsync<TResult>(string cacheKey, CancellationToken cancellationToken)
        {
            var cachedResponse = await this.cacheService.GetAsync<List<TResult>>(cacheKey, cancellationToken);
            if (!cachedResponse.IsNull && cachedResponse.HasValue)
            {
                this.logger.LogInformation("Fetch data from cache with cacheKey: {cacheKey}", cacheKey);

                return Result.Success(cachedResponse.Value);
            }

            return Result.NotFound();
        }

        private async Task CacheSetAsync<TIn>(string cacheKey, TIn response, CancellationToken cancellationToken)
        {
            await this.cacheService.SetAsync(cacheKey, response, cancellationToken: cancellationToken);

            this.logger.LogInformation("Set data to cache with  cacheKey: {cacheKey}", cacheKey);
        }

        private async Task CacheSetListAsync(string cacheKey, List<T> response, CancellationToken cancellationToken)
        {
            await this.cacheService.SetAsync(cacheKey, response, cancellationToken: cancellationToken);

            this.logger.LogInformation("Set data to cache with  cacheKey: {cacheKey}", cacheKey);
        }

        private async Task CacheSetResultAsync<TResult>(string cacheKey, TResult response, CancellationToken cancellationToken)
        {
            await this.cacheService.SetAsync(cacheKey, response, cancellationToken: cancellationToken);
            this.logger.LogInformation("Set data to cache with  cacheKey: {cacheKey}", cacheKey);
        }

        private async Task CacheSetListResultAsync<TResult>(string cacheKey, List<TResult> response, CancellationToken cancellationToken)
        {
            await this.cacheService.SetAsync(cacheKey, response, cancellationToken: cancellationToken);
            this.logger.LogInformation("Set data to cache with  cacheKey: {cacheKey}", cacheKey);
        }

        private async Task CacheServiceRemoveByPrefixAsync(CancellationToken cancellationToken)
        {
            await this.cacheService.RemoveByPrefixAsync(this.CacheKey, cancellationToken);

            this.logger.LogInformation("Cache data with cacheKey: {cacheKey} removed.", this.CacheKey);
        }
    }
}
