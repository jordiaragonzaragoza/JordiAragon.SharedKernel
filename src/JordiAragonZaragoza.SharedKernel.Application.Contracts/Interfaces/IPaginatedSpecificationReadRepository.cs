namespace JordiAragonZaragoza.SharedKernel.Application.Contracts.Interfaces
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;
    using JordiAragonZaragoza.SharedKernel.Contracts.Repositories;

    public interface IPaginatedSpecificationReadRepository<TReadModel> : ISpecificationReadRepository<TReadModel, Guid>
        where TReadModel : class, IReadModel
    {
        Task<PaginatedCollectionOutputDto<TReadModel>> PaginatedListAsync(IPaginatedSpecification<TReadModel> paginatedSpecification, CancellationToken cancellationToken);
    }
}