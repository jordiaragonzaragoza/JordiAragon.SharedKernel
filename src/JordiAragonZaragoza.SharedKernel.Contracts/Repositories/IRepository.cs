namespace JordiAragonZaragoza.SharedKernel.Contracts.Repositories
{
    using System.Threading;
    using System.Threading.Tasks;
    using JordiAragonZaragoza.SharedKernel.Contracts.Model;

    public interface IRepository<TModel, in TId> : IReadRepository<TModel, TId>
        where TModel : class, IBaseModel<TId>
        where TId : notnull
    {
        Task<TModel> AddAsync(TModel aggregate, CancellationToken cancellationToken = default);

        Task UpdateAsync(TModel aggregate, CancellationToken cancellationToken = default);

        Task DeleteAsync(TModel aggregate, CancellationToken cancellationToken = default);
    }
}