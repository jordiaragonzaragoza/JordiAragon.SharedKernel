namespace JordiAragon.SharedKernel.Infrastructure.EntityFramework.Repositories.ReadModel
{
    using System;
    using System.Collections.Generic;
    using System.Threading;
    using System.Threading.Tasks;
    using Ardalis.GuardClauses;
    using Ardalis.Result;
    using Ardalis.Specification;
    using JordiAragon.SharedKernel.Application.Contracts.Interfaces;
    using JordiAragon.SharedKernel.Contracts.DependencyInjection;
    using JordiAragon.SharedKernel.Contracts.Repositories;
    using Microsoft.Extensions.Logging;

    public abstract class BaseCachedSpecificationRepository<TReadModel> : BaseReadRepository<TReadModel>, ICachedSpecificationRepository<TReadModel, Guid>, IScopedDependency
        where TReadModel : class, IReadModel
    {
        private readonly ICacheService cacheService;
        private readonly ILogger<BaseCachedSpecificationRepository<TReadModel>> logger;

        protected BaseCachedSpecificationRepository(
            BaseReadModelContext dbContext,
            ILogger<BaseCachedSpecificationRepository<TReadModel>> logger,
            ICacheService cacheService)
            : base(dbContext)
        {
            this.cacheService = Guard.Against.Null(cacheService, nameof(cacheService));
            this.logger = Guard.Against.Null(logger, nameof(logger));
        }

        public string CacheKey => $"{typeof(TReadModel)}";

        public override async Task<TReadModel> AddAsync(TReadModel entity, CancellationToken cancellationToken = default)
        {
            var response = await base.AddAsync(entity, cancellationToken);

            await this.CacheServiceRemoveByPrefixAsync(cancellationToken);

            return response;
        }

        public override async Task<IEnumerable<TReadModel>> AddRangeAsync(IEnumerable<TReadModel> entities, CancellationToken cancellationToken = default)
        {
            var response = await base.AddRangeAsync(entities, cancellationToken);

            await this.CacheServiceRemoveByPrefixAsync(cancellationToken);

            return response;
        }

        public override async Task UpdateAsync(TReadModel entity, CancellationToken cancellationToken = default)
        {
            await base.UpdateAsync(entity, cancellationToken);

            await this.CacheServiceRemoveByPrefixAsync(cancellationToken);
        }

        public override async Task UpdateRangeAsync(IEnumerable<TReadModel> entities, CancellationToken cancellationToken = default)
        {
            await base.UpdateRangeAsync(entities, cancellationToken);

            await this.CacheServiceRemoveByPrefixAsync(cancellationToken);
        }

        public override async Task DeleteAsync(TReadModel entity, CancellationToken cancellationToken = default)
        {
            await base.DeleteAsync(entity, cancellationToken);

            await this.CacheServiceRemoveByPrefixAsync(cancellationToken);
        }

        public override async Task DeleteRangeAsync(IEnumerable<TReadModel> entities, CancellationToken cancellationToken = default)
        {
            await base.DeleteRangeAsync(entities, cancellationToken);

            await this.CacheServiceRemoveByPrefixAsync(cancellationToken);
        }

        public override async Task<TReadModel> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
        {
            var cacheKeyId = $"{this.CacheKey}_{id}";

            var cachedResponse = await this.CacheGetAsync<TReadModel>(cacheKeyId, cancellationToken);
            if (cachedResponse.IsSuccess)
            {
                return cachedResponse;
            }

            var response = await base.GetByIdAsync(id, cancellationToken);

            await this.CacheSetAsync(cacheKeyId, response, cancellationToken);

            return response;
        }

        public override async Task<TReadModel> FirstOrDefaultAsync(ISpecification<TReadModel> specification, CancellationToken cancellationToken = default)
        {
            var cacheKeySpecification = $"{this.CacheKey}_{specification.GetType().FullName}";

            var cachedResponse = await this.CacheGetAsync<TReadModel>(cacheKeySpecification, cancellationToken);
            if (cachedResponse.IsSuccess)
            {
                return cachedResponse;
            }

            var response = await base.FirstOrDefaultAsync(specification, cancellationToken);

            await this.CacheSetAsync(cacheKeySpecification, response, cancellationToken);

            return response;
        }

        public override async Task<TResult> FirstOrDefaultAsync<TResult>(ISpecification<TReadModel, TResult> specification, CancellationToken cancellationToken = default)
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

        public override async Task<TReadModel> SingleOrDefaultAsync(ISingleResultSpecification<TReadModel> specification, CancellationToken cancellationToken = default)
        {
            var cacheKeySpecification = $"{this.CacheKey}_{specification.GetType().FullName}";
            var cachedResponse = await this.CacheGetAsync<TReadModel>(cacheKeySpecification, cancellationToken);
            if (cachedResponse.IsSuccess)
            {
                return cachedResponse;
            }

            var response = await base.SingleOrDefaultAsync(specification, cancellationToken);

            await this.CacheSetAsync(cacheKeySpecification, response, cancellationToken);

            return response;
        }

        public override async Task<TResult> SingleOrDefaultAsync<TResult>(ISingleResultSpecification<TReadModel, TResult> specification, CancellationToken cancellationToken = default)
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

        public override async Task<List<TReadModel>> ListAsync(CancellationToken cancellationToken = default)
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

        public override async Task<List<TReadModel>> ListAsync(ISpecification<TReadModel> specification, CancellationToken cancellationToken = default)
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

        public override async Task<List<TResult>> ListAsync<TResult>(ISpecification<TReadModel, TResult> specification, CancellationToken cancellationToken = default)
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

        public override async Task<int> CountAsync(ISpecification<TReadModel> specification, CancellationToken cancellationToken = default)
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

        public override async Task<bool> AnyAsync(ISpecification<TReadModel> specification, CancellationToken cancellationToken = default)
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

        private async Task<Result<List<TReadModel>>> CacheGetListAsync(string cacheKey, CancellationToken cancellationToken)
        {
            var cachedResponse = await this.cacheService.GetAsync<List<TReadModel>>(cacheKey, cancellationToken);
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

        private async Task CacheSetListAsync(string cacheKey, List<TReadModel> response, CancellationToken cancellationToken)
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
