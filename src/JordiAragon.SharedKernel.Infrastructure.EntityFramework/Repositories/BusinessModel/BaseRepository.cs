namespace JordiAragon.SharedKernel.Infrastructure.EntityFramework.Repositories.BusinessModel
{
    using JordiAragon.SharedKernel.Contracts.DependencyInjection;
    using JordiAragon.SharedKernel.Domain.Contracts.Interfaces;
    using JordiAragon.SharedKernel.Domain.Entities;

    public abstract class BaseRepository<TAggregate, TId> : BaseReadRepository<TAggregate, TId>, IScopedDependency
        where TAggregate : BaseAggregateRoot<TId>
        where TId : class, IEntityId
    {
        protected BaseRepository(BaseBusinessModelContext dbContext)
            : base(dbContext)
        {
        }
    }
}
