namespace JordiAragonZaragoza.SharedKernel.Contracts.Repositories
{
    using JordiAragonZaragoza.SharedKernel.Contracts.Model;

    public interface ICachedSpecificationRepository<TModel, TId> : IRangeableRepository<TModel, TId>, ISpecificationReadRepository<TModel, TId>
        where TModel : class, IBaseModel<TId>
        where TId : notnull
    {
        string CacheKey { get; }
    }
}