namespace JordiAragon.SharedKernel.Domain.Contracts.Interfaces
{
    using Ardalis.Specification;

    public interface ICachedRepository<T> : IRepositoryBase<T>
        where T : class, IAggregateRoot
    {
        string CacheKey { get; }
    }
}
