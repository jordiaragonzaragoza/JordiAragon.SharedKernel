namespace JordiAragon.SharedKernel.Domain.Contracts.Interfaces
{
    using Ardalis.Specification;

    public interface ICachedRepository<T, TId> : IRepositoryBase<T>
        where T : class, IAggregateRoot<TId>
    {
        string CacheKey { get; }
    }
}
