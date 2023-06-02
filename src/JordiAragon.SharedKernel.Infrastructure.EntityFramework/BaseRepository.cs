namespace JordiAragon.SharedKernel.Infrastructure.EntityFramework
{
    using Ardalis.Specification.EntityFrameworkCore;
    using JordiAragon.SharedKernel.Contracts.DependencyInjection;
    using JordiAragon.SharedKernel.Domain.Contracts.Interfaces;

    public abstract class BaseRepository<T> : RepositoryBase<T>, IRepository<T>, IScopedDependency
        where T : class, IAggregateRoot
    {
        protected BaseRepository(BaseContext dbContext)
            : base(dbContext)
        {
        }
    }
}
