namespace JordiAragon.SharedKernel.Infrastructure.EntityFramework.Repositories
{
    using System.Collections.Generic;
    using System.Threading;
    using System.Threading.Tasks;
    using Ardalis.GuardClauses;
    using Ardalis.Result;
    using Ardalis.Specification;
    using JordiAragon.SharedKernel.Application.Contracts.Interfaces;
    using JordiAragon.SharedKernel.Contracts.DependencyInjection;
    using JordiAragon.SharedKernel.Domain.Contracts.Interfaces;
    using Microsoft.Extensions.Logging;

    public abstract class BaseCachedSpecificationRepository<TAggregate, TId> : BaseReadRepository<TAggregate, TId>, ICachedSpecificationRepository<TAggregate, TId>, IScopedDependency
        where TAggregate : class, IAggregateRoot<TId>
        where TId : class, IEntityId
    {
        private readonly ICacheService cacheService;
        private readonly ILogger<BaseCachedSpecificationRepository<TAggregate, TId>> logger;

        protected BaseCachedSpecificationRepository(
            BaseContext dbContext,
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

        public override async Task<TAggregate> GetByIdAsync(TId id, CancellationToken cancellationToken = default)
        {
            var cacheKeyId = $"{this.CacheKey}_{id}";

            var cachedResponse = await this.CacheGetAsync<TAggregate>(cacheKeyId, cancellationToken);
            if (cachedResponse.IsSuccess)
            {
                return cachedResponse;
            }

            var response = await base.GetByIdAsync(id, cancellationToken);

            await this.CacheSetAsync(cacheKeyId, response, cancellationToken);

            return response;
        }

        /*public override async Task<TAggregate> GetByIdAsync<TIdentifier>(TIdentifier id, CancellationToken cancellationToken = default)
        {
            var cacheKeyId = $"{this.CacheKey}_{id}";

            var cachedResponse = await this.CacheGetAsync<TAggregate>(cacheKeyId, cancellationToken);
            if (cachedResponse.IsSuccess)
            {
                return cachedResponse;
            }

            var response = await base.GetByIdAsync(id, cancellationToken);

            await this.CacheSetAsync(cacheKeyId, response, cancellationToken);

            return response;
        }*/

        public override async Task<TAggregate> FirstOrDefaultAsync(ISpecification<TAggregate> specification, CancellationToken cancellationToken = default)
        {
            var cacheKeySpecification = $"{this.CacheKey}_{specification.GetType().FullName}";

            var cachedResponse = await this.CacheGetAsync<TAggregate>(cacheKeySpecification, cancellationToken);
            if (cachedResponse.IsSuccess)
            {
                return cachedResponse;
            }

            var response = await base.FirstOrDefaultAsync(specification, cancellationToken);

            await this.CacheSetAsync(cacheKeySpecification, response, cancellationToken);

            return response;
        }

        public override async Task<TResult> FirstOrDefaultAsync<TResult>(ISpecification<TAggregate, TResult> specification, CancellationToken cancellationToken = default)
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

        public override async Task<TAggregate> SingleOrDefaultAsync(ISingleResultSpecification<TAggregate> specification, CancellationToken cancellationToken = default)
        {
            var cacheKeySpecification = $"{this.CacheKey}_{specification.GetType().FullName}";
            var cachedResponse = await this.CacheGetAsync<TAggregate>(cacheKeySpecification, cancellationToken);
            if (cachedResponse.IsSuccess)
            {
                return cachedResponse;
            }

            var response = await base.SingleOrDefaultAsync(specification, cancellationToken);

            await this.CacheSetAsync(cacheKeySpecification, response, cancellationToken);

            return response;
        }

        public override async Task<TResult> SingleOrDefaultAsync<TResult>(ISingleResultSpecification<TAggregate, TResult> specification, CancellationToken cancellationToken = default)
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

        public override async Task<List<TAggregate>> ListAsync(CancellationToken cancellationToken = default)
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

        public override async Task<List<TAggregate>> ListAsync(ISpecification<TAggregate> specification, CancellationToken cancellationToken = default)
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

        public override async Task<List<TResult>> ListAsync<TResult>(ISpecification<TAggregate, TResult> specification, CancellationToken cancellationToken = default)
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

        public override async Task<int> CountAsync(ISpecification<TAggregate> specification, CancellationToken cancellationToken = default)
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

        public override async Task<bool> AnyAsync(ISpecification<TAggregate> specification, CancellationToken cancellationToken = default)
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

        private async Task<Result<List<TAggregate>>> CacheGetListAsync(string cacheKey, CancellationToken cancellationToken)
        {
            var cachedResponse = await this.cacheService.GetAsync<List<TAggregate>>(cacheKey, cancellationToken);
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

        private async Task CacheSetListAsync(string cacheKey, List<TAggregate> response, CancellationToken cancellationToken)
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
