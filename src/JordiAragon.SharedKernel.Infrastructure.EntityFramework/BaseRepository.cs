namespace JordiAragon.SharedKernel.Infrastructure.EntityFramework
{
    using Ardalis.Specification.EntityFrameworkCore;
    using JordiAragon.SharedKernel.Contracts.DependencyInjection;
    using JordiAragon.SharedKernel.Domain.Contracts.Interfaces;

    public abstract class BaseRepository<T, TId> : RepositoryBase<T>, IRepository<T, TId>, IScopedDependency
        where T : class, IAggregateRoot<TId>
    {
        protected BaseRepository(BaseContext dbContext)
            : base(dbContext)
        {
        }
    }
}
