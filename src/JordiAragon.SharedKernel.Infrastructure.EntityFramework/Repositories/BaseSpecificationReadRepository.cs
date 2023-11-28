namespace JordiAragon.SharedKernel.Infrastructure.EntityFramework.Repositories
{
    using JordiAragon.SharedKernel.Contracts.DependencyInjection;
    using JordiAragon.SharedKernel.Domain.Contracts.Interfaces;

    public abstract class BaseSpecificationReadRepository<TEntity, TId, TIdType> : BaseReadRepository<TEntity, TId, TIdType>, ISpecificationReadRepository<TEntity, TId, TIdType>, IScopedDependency
        where TEntity : class, IEntity<TId, TIdType>
        where TId : class, IEntityId<TIdType>
    {
        protected BaseSpecificationReadRepository(BaseContext dbContext)
            : base(dbContext)
        {
        }
    }
}