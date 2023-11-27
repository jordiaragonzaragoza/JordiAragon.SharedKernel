namespace JordiAragon.SharedKernel.Infrastructure.EntityFramework
{
    using Ardalis.Specification.EntityFrameworkCore;
    using JordiAragon.SharedKernel.Contracts.DependencyInjection;
    using JordiAragon.SharedKernel.Domain.Contracts.Interfaces;

    public abstract class BaseReadRepository<TEntity, TId, TIdType> : RepositoryBase<TEntity>, IReadRepository<TEntity, TId, TIdType>, IScopedDependency
        where TEntity : class, IEntity<TId, TIdType>
        where TId : IEntityId<TIdType>
    {
        protected BaseReadRepository(BaseContext dbContext)
            : base(dbContext)
        {
        }
    }
}