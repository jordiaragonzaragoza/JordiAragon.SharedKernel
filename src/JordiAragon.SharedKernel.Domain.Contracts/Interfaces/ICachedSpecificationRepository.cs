namespace JordiAragon.SharedKernel.Domain.Contracts.Interfaces
{
    public interface ICachedSpecificationRepository<TAggregate, TId, TIdType> : IRepository<TAggregate, TId, TIdType>, ISpecificationReadRepository<TAggregate, TId, TIdType>
        where TAggregate : class, IAggregateRoot<TId, TIdType>
        where TId : IEntityId<TIdType>
    {
        string CacheKey { get; }
    }
}
