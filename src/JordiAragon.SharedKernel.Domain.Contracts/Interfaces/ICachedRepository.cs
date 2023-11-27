namespace JordiAragon.SharedKernel.Domain.Contracts.Interfaces
{
    public interface ICachedRepository<TAggregate, TId, TIdType> : IReadRepository<TAggregate, TId, TIdType>, IRepository<TAggregate, TId, TIdType>
        where TAggregate : class, IAggregateRoot<TId, TIdType>
        where TId : IEntityId<TIdType>
    {
        string CacheKey { get; }
    }
}
