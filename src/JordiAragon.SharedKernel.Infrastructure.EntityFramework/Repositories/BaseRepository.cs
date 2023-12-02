namespace JordiAragon.SharedKernel.Infrastructure.EntityFramework.Repositories
{
    using JordiAragon.SharedKernel.Contracts.DependencyInjection;
    using JordiAragon.SharedKernel.Domain.Contracts.Interfaces;

    public abstract class BaseRepository<TAggregate, TId> : BaseReadRepository<TAggregate, TId>, IRepository<TAggregate, TId>, IScopedDependency
        where TAggregate : class, IAggregateRoot<TId>
        where TId : class, IEntityId
    {
        protected BaseRepository(BaseContext dbContext)
            : base(dbContext)
        {
        }
    }
}
