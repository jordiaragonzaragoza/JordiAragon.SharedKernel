namespace JordiAragon.SharedKernel.Infrastructure.EntityFramework.Repositories.DataModel
{
    using System;
    using JordiAragon.SharedKernel.Contracts.DependencyInjection;
    using JordiAragon.SharedKernel.Contracts.Repositories;
    using JordiAragon.SharedKernel.Infrastructure.Interfaces;

    public abstract class BaseRepository<TDataEntity> : BaseReadRepository<TDataEntity>, IRangeableRepository<TDataEntity, Guid>, IScopedDependency
        where TDataEntity : class, IDataEntity
    {
        protected BaseRepository(BaseBusinessModelContext dbContext)
            : base(dbContext)
        {
        }
    }
}
