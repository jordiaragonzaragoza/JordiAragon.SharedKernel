namespace JordiAragon.SharedKernel.Infrastructure.EntityFramework.Repositories.ReadModel
{
    using System;
    using JordiAragon.SharedKernel.Application.Contracts.Interfaces;
    using JordiAragon.SharedKernel.Contracts.DependencyInjection;
    using JordiAragon.SharedKernel.Contracts.Repositories;
    using JordiAragon.SharedKernel.Infrastructure.EntityFramework.Context;

    public abstract class BaseRepository<TReadModel> : BaseReadRepository<TReadModel>, IRangeableRepository<TReadModel, Guid>, IScopedDependency
        where TReadModel : class, IReadModel
    {
        protected BaseRepository(BaseReadModelContext dbContext)
            : base(dbContext)
        {
        }
    }
}