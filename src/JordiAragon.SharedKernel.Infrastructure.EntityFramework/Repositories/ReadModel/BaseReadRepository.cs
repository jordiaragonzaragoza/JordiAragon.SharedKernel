namespace JordiAragon.SharedKernel.Infrastructure.EntityFramework.Repositories.ReadModel
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;
    using Ardalis.GuardClauses;
    using Ardalis.Specification;
    using Ardalis.Specification.EntityFrameworkCore;
    using JordiAragon.SharedKernel.Application.Contracts;
    using JordiAragon.SharedKernel.Application.Contracts.Interfaces;
    using JordiAragon.SharedKernel.Contracts.DependencyInjection;
    using JordiAragon.SharedKernel.Contracts.Repositories;
    using Microsoft.EntityFrameworkCore;

    public abstract class BaseReadRepository<TReadModel> : RepositoryBase<TReadModel>, IPaginatedSpecificationReadRepository<TReadModel>, IRangeableRepository<TReadModel, Guid>, IScopedDependency
        where TReadModel : class, IReadModel
    {
        protected BaseReadRepository(BaseReadModelContext readContext)
            : base(readContext)
        {
        }

        public virtual Task<TReadModel> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
        {
            return this.GetByIdAsync<Guid>(id, cancellationToken);
        }

        public async Task<PaginatedCollectionOutputDto<TReadModel>> PaginatedListAsync(IPaginatedSpecification<TReadModel> paginatedSpecification, CancellationToken cancellationToken)
        {
            Guard.Against.Null(paginatedSpecification);
            var request = paginatedSpecification.Request;

            var totalCount = await this.ApplySpecification(paginatedSpecification).CountAsync(cancellationToken);
            if (totalCount == 0)
            {
                return new PaginatedCollectionOutputDto<TReadModel>(default, default, totalCount, new List<TReadModel>());
            }

            var totalPages = request.PageSize > 0 ? (int)Math.Ceiling(totalCount / (double)request.PageSize) : 1;

            var actualPage = request.PageNumber == 0 ? 1 : request.PageNumber;
            actualPage = actualPage > totalPages ? totalPages : actualPage;
            actualPage = request.PageSize == 0 ? 1 : actualPage;

            var query = this.ApplySpecification(paginatedSpecification);

            if (actualPage > 1)
            {
                var skip = (actualPage - 1) * request.PageSize;
                query = query.Skip(skip);
            }

            if (request.PageSize > 0)
            {
                query = query.Take(request.PageSize);
            }

            var items = await query.ToListAsync(cancellationToken);

            return new PaginatedCollectionOutputDto<TReadModel>(actualPage, totalPages, totalCount, items);
        }
    }
}