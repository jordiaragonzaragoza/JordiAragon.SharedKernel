namespace JordiAragon.SharedKernel.Infrastructure.EntityFramework.Repositories.BusinessModel
{
    using System.Collections.Generic;
    using System.Threading;
    using System.Threading.Tasks;
    using Ardalis.GuardClauses;
    using Ardalis.Specification;
    using JordiAragon.SharedKernel.Application.Contracts.Interfaces;
    using JordiAragon.SharedKernel.Contracts.DependencyInjection;
    using JordiAragon.SharedKernel.Contracts.Repositories;
    using JordiAragon.SharedKernel.Domain.Contracts.Interfaces;
    using JordiAragon.SharedKernel.Infrastructure.EntityFramework.Context;
    using Microsoft.Extensions.Logging;

    public abstract class BaseCachedSpecificationRepository<TAggregate, TId> : BaseReadRepository<TAggregate, TId>, ICachedSpecificationRepository<TAggregate, TId>, IScopedDependency
        where TAggregate : class, IAggregateRoot<TId>
        where TId : class, IEntityId
    {
        private readonly ICacheService cacheService;
        private readonly ILogger<BaseCachedSpecificationRepository<TAggregate, TId>> logger;

        protected BaseCachedSpecificationRepository(
            BaseBusinessModelContext dbContext,
            ILogger<BaseCachedSpecificationRepository<TAggregate, TId>> logger,
            ICacheService cacheService)
            : base(dbContext)
        {
            this.cacheService = Guard.Against.Null(cacheService, nameof(cacheService));
            this.logger = Guard.Against.Null(logger, nameof(logger));
        }

        public string CacheKey => $"{typeof(TAggregate)}";

        public override async Task<TAggregate> AddAsync(TAggregate entity, CancellationToken cancellationToken = default)
        {
            var response = await base.AddAsync(entity, cancellationToken);

            await this.CacheServiceRemoveByPrefixAsync(cancellationToken);

            return response;
        }

        public override async Task<IEnumerable<TAggregate>> AddRangeAsync(IEnumerable<TAggregate> entities, CancellationToken cancellationToken = default)
        {
            var response = await base.AddRangeAsync(entities, cancellationToken);

            await this.CacheServiceRemoveByPrefixAsync(cancellationToken);

            return response;
        }

        public override async Task UpdateAsync(TAggregate entity, CancellationToken cancellationToken = default)
        {
            await base.UpdateAsync(entity, cancellationToken);

            await this.CacheServiceRemoveByPrefixAsync(cancellationToken);
        }

        public override async Task UpdateRangeAsync(IEnumerable<TAggregate> entities, CancellationToken cancellationToken = default)
        {
            await base.UpdateRangeAsync(entities, cancellationToken);

            await this.CacheServiceRemoveByPrefixAsync(cancellationToken);
        }

        public override async Task DeleteAsync(TAggregate entity, CancellationToken cancellationToken = default)
        {
            await base.DeleteAsync(entity, cancellationToken);

            await this.CacheServiceRemoveByPrefixAsync(cancellationToken);
        }

        public override async Task DeleteRangeAsync(IEnumerable<TAggregate> entities, CancellationToken cancellationToken = default)
        {
            await base.DeleteRangeAsync(entities, cancellationToken);

            await this.CacheServiceRemoveByPrefixAsync(cancellationToken);
        }

        public override async Task<TAggregate?> GetByIdAsync(TId id, CancellationToken cancellationToken = default)
        {
            var cacheKeyId = $"{this.CacheKey}_{id}";

            var cacheResponse = await this.CacheGetAsync<TAggregate>(cacheKeyId, cancellationToken);
            if (cacheResponse != null)
            {
                return cacheResponse;
            }

            var response = await base.GetByIdAsync(id, cancellationToken);

            await this.CacheSetAsync(cacheKeyId, response, cancellationToken);

            return response;
        }

        public override async Task<TAggregate?> FirstOrDefaultAsync(ISpecification<TAggregate> specification, CancellationToken cancellationToken = default)
        {
            var cacheKeySpecification = $"{this.CacheKey}_{specification.GetType().FullName}";

            var cachedResponse = await this.CacheGetAsync<TAggregate>(cacheKeySpecification, cancellationToken);
            if (cachedResponse != null)
            {
                return cachedResponse;
            }

            var response = await base.FirstOrDefaultAsync(specification, cancellationToken);

            await this.CacheSetAsync(cacheKeySpecification, response, cancellationToken);

            return response;
        }

        public override async Task<TResult?> FirstOrDefaultAsync<TResult>(ISpecification<TAggregate, TResult> specification, CancellationToken cancellationToken = default)
            where TResult : default
        {
            var cacheKeySpecification = $"{this.CacheKey}_{specification.GetType().FullName}";
            var cachedResponse = await this.cacheService.GetAsync<TResult>(cacheKeySpecification, cancellationToken);
            if (!cachedResponse.IsNull && cachedResponse.HasValue)
            {
                this.logger.LogInformation("Fetch data from cache with cacheKey: {cacheKey}", cacheKeySpecification);

                return cachedResponse.Value;
            }

            var response = await base.FirstOrDefaultAsync(specification, cancellationToken);

            await this.CacheSetAsync(cacheKeySpecification, response, cancellationToken);

            return response;
        }

        public override async Task<TAggregate?> SingleOrDefaultAsync(ISingleResultSpecification<TAggregate> specification, CancellationToken cancellationToken = default)
        {
            var cacheKeySpecification = $"{this.CacheKey}_{specification.GetType().FullName}";
            var cachedResponse = await this.CacheGetAsync<TAggregate>(cacheKeySpecification, cancellationToken);
            if (cachedResponse != null)
            {
                return cachedResponse;
            }

            var response = await base.SingleOrDefaultAsync(specification, cancellationToken);

            await this.CacheSetAsync(cacheKeySpecification, response, cancellationToken);

            return response;
        }

        public override async Task<TResult?> SingleOrDefaultAsync<TResult>(ISingleResultSpecification<TAggregate, TResult> specification, CancellationToken cancellationToken = default)
            where TResult : default
        {
            var cacheKeySpecification = $"{this.CacheKey}_{specification.GetType().FullName}";

            var cachedResponse = await this.cacheService.GetAsync<TResult>(cacheKeySpecification, cancellationToken);
            if (!cachedResponse.IsNull && cachedResponse.HasValue)
            {
                this.logger.LogInformation("Fetch data from cache with cacheKey: {cacheKey}", cacheKeySpecification);

                return cachedResponse.Value;
            }

            var response = await base.SingleOrDefaultAsync(specification, cancellationToken);

            await this.CacheSetAsync(cacheKeySpecification, response, cancellationToken);

            return response;
        }

        public override async Task<List<TAggregate>> ListAsync(CancellationToken cancellationToken = default)
        {
            var cachedResponse = await this.CacheGetListAsync<TAggregate>(this.CacheKey, cancellationToken);
            if (cachedResponse != null)
            {
                return cachedResponse;
            }

            var response = await base.ListAsync(cancellationToken);

            await this.CacheSetListAsync(this.CacheKey, response, cancellationToken);

            return response;
        }

        public override async Task<List<TAggregate>> ListAsync(ISpecification<TAggregate> specification, CancellationToken cancellationToken = default)
        {
            var cacheKeySpecification = $"{this.CacheKey}_{specification.GetType().FullName}";
            var cachedResponse = await this.CacheGetListAsync<TAggregate>(cacheKeySpecification, cancellationToken);
            if (cachedResponse != null)
            {
                return cachedResponse;
            }

            var response = await base.ListAsync(specification, cancellationToken);

            await this.CacheSetListAsync(cacheKeySpecification, response, cancellationToken);

            return response;
        }

        public override async Task<List<TResult>> ListAsync<TResult>(ISpecification<TAggregate, TResult> specification, CancellationToken cancellationToken = default)
        {
            var cacheKeySpecification = $"{this.CacheKey}_{specification.GetType().FullName}";
            var cachedResponse = await this.CacheGetListAsync<TResult>(cacheKeySpecification, cancellationToken);
            if (cachedResponse != null)
            {
                return cachedResponse;
            }

            var response = await base.ListAsync(specification, cancellationToken);

            await this.CacheSetListAsync(cacheKeySpecification, response, cancellationToken);

            return response;
        }

        public override async Task<int> CountAsync(ISpecification<TAggregate> specification, CancellationToken cancellationToken = default)
        {
            var cacheKeySpecification = $"{this.CacheKey}_{specification.GetType().FullName}";

            var cachedResponse = await this.cacheService.GetAsync<int>(cacheKeySpecification, cancellationToken);
            if (!cachedResponse.IsNull && cachedResponse.HasValue)
            {
                this.logger.LogInformation("Fetch data from cache with cacheKey: {cacheKey}", cacheKeySpecification);

                return cachedResponse.Value;
            }

            var response = await base.CountAsync(specification, cancellationToken);

            await this.CacheSetAsync(cacheKeySpecification, response, cancellationToken);

            return response;
        }

        public override async Task<int> CountAsync(CancellationToken cancellationToken = default)
        {
            var cachedResponse = await this.cacheService.GetAsync<int>(this.CacheKey, cancellationToken);
            if (!cachedResponse.IsNull && cachedResponse.HasValue)
            {
                this.logger.LogInformation("Fetch data from cache with cacheKey: {cacheKey}", this.CacheKey);

                return cachedResponse.Value;
            }

            var response = await base.CountAsync(cancellationToken);

            await this.CacheSetAsync(this.CacheKey, response, cancellationToken);

            return response;
        }

        public override async Task<bool> AnyAsync(ISpecification<TAggregate> specification, CancellationToken cancellationToken = default)
        {
            var cacheKeySpecification = $"{this.CacheKey}_{specification.GetType().FullName}";
            var cachedResponse = await this.cacheService.GetAsync<bool>(cacheKeySpecification, cancellationToken);
            if (!cachedResponse.IsNull && cachedResponse.HasValue)
            {
                this.logger.LogInformation("Fetch data from cache with cacheKey: {cacheKey}", cacheKeySpecification);

                return cachedResponse.Value;
            }

            var response = await base.AnyAsync(specification, cancellationToken);

            await this.CacheSetAsync(cacheKeySpecification, response, cancellationToken);

            return response;
        }

        public override async Task<bool> AnyAsync(CancellationToken cancellationToken = default)
        {
            var cachedResponse = await this.cacheService.GetAsync<bool>(this.CacheKey, cancellationToken);
            if (!cachedResponse.IsNull && cachedResponse.HasValue)
            {
                this.logger.LogInformation("Fetch data from cache with cacheKey: {cacheKey}", this.CacheKey);

                return cachedResponse.Value;
            }

            var response = await base.AnyAsync(cancellationToken);

            await this.CacheSetAsync(this.CacheKey, response, cancellationToken);

            return response;
        }

        private async Task<T?> CacheGetAsync<T>(string cacheKey, CancellationToken cancellationToken)
            where T : class
        {
            var cachedResponse = await this.cacheService.GetAsync<T>(cacheKey, cancellationToken);
            if (!cachedResponse.IsNull && cachedResponse.HasValue)
            {
                this.logger.LogInformation("Fetch data from cache with cacheKey: {cacheKey}", cacheKey);

                return cachedResponse.Value;
            }

            return default;
        }

        private async Task<List<TIn>?> CacheGetListAsync<TIn>(string cacheKey, CancellationToken cancellationToken)
        {
            var cachedResponse = await this.cacheService.GetAsync<List<TIn>>(cacheKey, cancellationToken);
            if (!cachedResponse.IsNull && cachedResponse.HasValue)
            {
                this.logger.LogInformation("Fetch data from cache with cacheKey: {cacheKey}", cacheKey);

                return cachedResponse.Value;
            }

            return default;
        }

        private async Task CacheSetAsync<TIn>(string cacheKey, TIn? response, CancellationToken cancellationToken)
        {
            await this.cacheService.SetAsync(cacheKey, response, cancellationToken: cancellationToken);

            this.logger.LogInformation("Set data to cache with  cacheKey: {cacheKey}", cacheKey);
        }

        private async Task CacheSetListAsync<TIn>(string cacheKey, List<TIn> response, CancellationToken cancellationToken)
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
