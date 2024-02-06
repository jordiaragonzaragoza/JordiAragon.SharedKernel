namespace JordiAragon.SharedKernel.Infrastructure.EntityFramework.Repositories.ReadModel
{
    using System;
    using JordiAragon.SharedKernel.Application.Contracts.Interfaces;
    using JordiAragon.SharedKernel.Contracts.DependencyInjection;
    using JordiAragon.SharedKernel.Contracts.Repositories;

    public abstract class BaseRepository<TReadModel> : BaseReadRepository<TReadModel>, IRangeableRepository<TReadModel, Guid>, IScopedDependency
        where TReadModel : class, IReadModel
    {
        protected BaseRepository(BaseReadContext dbContext)
            : base(dbContext)
        {
        }
    }
}
