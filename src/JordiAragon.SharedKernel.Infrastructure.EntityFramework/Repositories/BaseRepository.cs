namespace JordiAragon.SharedKernel.Infrastructure.EntityFramework.Repositories
{
    using JordiAragon.SharedKernel.Contracts.DependencyInjection;
    using JordiAragon.SharedKernel.Domain.Contracts.Interfaces;
    using JordiAragon.SharedKernel.Domain.Entities;

    public abstract class BaseRepository<TAggregate, TId> : BaseReadRepository<TAggregate, TId>, IRangeableRepository<TAggregate, TId>, IScopedDependency
        where TAggregate : BaseAggregateRoot<TId>
        where TId : class, IEntityId
    {
        protected BaseRepository(BaseContext dbContext)
            : base(dbContext)
        {
        }
    }
}
