namespace JordiAragon.SharedKernel.Infrastructure.EntityFramework.Repositories
{
    using JordiAragon.SharedKernel.Contracts.DependencyInjection;
    using JordiAragon.SharedKernel.Domain.Contracts.Interfaces;

    public abstract class BaseRepository<TAggregate, TId, TIdType> : BaseReadRepository<TAggregate, TId, TIdType>, IRepository<TAggregate, TId, TIdType>, IScopedDependency
        where TAggregate : class, IAggregateRoot<TId, TIdType>
        where TId : class, IEntityId<TIdType>
    {
        protected BaseRepository(BaseContext dbContext)
            : base(dbContext)
        {
        }
    }
}
