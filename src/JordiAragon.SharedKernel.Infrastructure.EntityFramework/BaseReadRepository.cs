namespace JordiAragon.SharedKernel.Infrastructure.EntityFramework
{
    using Ardalis.Specification.EntityFrameworkCore;
    using JordiAragon.SharedKernel.Contracts.DependencyInjection;
    using JordiAragon.SharedKernel.Domain.Contracts.Interfaces;

    public abstract class BaseReadRepository<T> : RepositoryBase<T>, IReadRepository<T>, IScopedDependency
        where T : class
    {
        protected BaseReadRepository(BaseContext dbContext)
            : base(dbContext)
        {
        }
    }
}
