namespace JordiAragon.SharedKernel.Infrastructure.EntityFramework.Repositories
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;
    using Ardalis.Specification.EntityFrameworkCore;
    using JordiAragon.SharedKernel.Contracts.DependencyInjection;
    using JordiAragon.SharedKernel.Domain.Contracts.Interfaces;
    using JordiAragon.SharedKernel.Domain.Entities;

    public abstract class BaseReadRepository<TEntity, TId> : RepositoryBase<TEntity>,  ISpecificationReadRepository<TEntity, TId>, IScopedDependency
        where TEntity : class, IEntity<TId>
        where TId : class, IEntityId
    {
        protected BaseReadRepository(BaseContext dbContext)
            : base(dbContext)
        {
        }

        public virtual Task<TEntity> GetByIdAsync(TId id, CancellationToken cancellationToken = default)
        {
            return this.FirstOrDefaultAsync(new EntityByIdSpec<TEntity, TId>(id), cancellationToken);
        }

        [Obsolete("This method is obsolete. Use GetByIdAsync instead.")]
        public new Task<TEntity> GetByIdAsync<TIdx>(TIdx id, CancellationToken cancellationToken = default)
        {
            throw new NotSupportedException("This method is obsolete. Use GetByIdAsync instead.");
        }
    }
}