namespace JordiAragon.SharedKernel.Infrastructure.EntityFramework.Repositories
{
    using JordiAragon.SharedKernel.Contracts.DependencyInjection;
    using JordiAragon.SharedKernel.Domain.Contracts.Interfaces;

    public abstract class BaseSpecificationReadRepository<TEntity, TId> : BaseReadRepository<TEntity, TId>, ISpecificationReadRepository<TEntity, TId>, IScopedDependency
        where TEntity : class, IEntity<TId>
        where TId : class, IEntityId
    {
        protected BaseSpecificationReadRepository(BaseContext dbContext)
            : base(dbContext)
        {
        }
    }
}