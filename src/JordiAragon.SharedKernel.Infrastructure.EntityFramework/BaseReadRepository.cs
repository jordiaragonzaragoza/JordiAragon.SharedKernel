namespace JordiAragon.SharedKernel.Infrastructure.EntityFramework
{
    using Ardalis.Specification.EntityFrameworkCore;
    using JordiAragon.SharedKernel.Contracts.DependencyInjection;
    using JordiAragon.SharedKernel.Domain.Contracts.Interfaces;

    public abstract class BaseReadRepository<T, TId> : RepositoryBase<T>, IReadRepository<T, TId>, IScopedDependency
        where T : class, IEntity<TId>
    {
        protected BaseReadRepository(BaseContext dbContext)
            : base(dbContext)
        {
        }
    }
}
