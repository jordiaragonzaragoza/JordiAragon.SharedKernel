namespace JordiAragon.SharedKernel.Contracts.Repositories
{
    using JordiAragon.SharedKernel.Contracts.Model;

    public interface ICachedSpecificationRepository<TModel, TId> : IRangeableRepository<TModel, TId>, ISpecificationReadRepository<TModel, TId>
        where TModel : class, IBaseModel<TId>
        where TId : notnull
    {
        string CacheKey { get; }
    }
}