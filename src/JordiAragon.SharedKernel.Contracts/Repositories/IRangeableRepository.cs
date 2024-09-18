namespace JordiAragon.SharedKernel.Contracts.Repositories
{
    using System.Collections.Generic;
    using System.Threading;
    using System.Threading.Tasks;
    using JordiAragon.SharedKernel.Contracts.Model;

    public interface IRangeableRepository<TModel, TId> : IRepository<TModel, TId>
        where TModel : class, IBaseModel<TId>
        where TId : notnull
    {
        Task<IEnumerable<TModel>> AddRangeAsync(IEnumerable<TModel> entities, CancellationToken cancellationToken = default);

        Task UpdateRangeAsync(IEnumerable<TModel> entities, CancellationToken cancellationToken = default);

        Task DeleteRangeAsync(IEnumerable<TModel> aggregates, CancellationToken cancellationToken = default);
    }
}