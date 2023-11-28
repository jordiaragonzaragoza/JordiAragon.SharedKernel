namespace JordiAragon.SharedKernel.Domain.Contracts.Interfaces
{
    public interface ICachedSpecificationRepository<TAggregate, TId> : IRepository<TAggregate, TId>, ISpecificationReadRepository<TAggregate, TId>
        where TAggregate : class, IAggregateRoot<TId>
        where TId : class, IEntityId
    {
        string CacheKey { get; }
    }
}
