namespace JordiAragon.SharedKernel.Infrastructure.EntityFramework.Repositories.DataModel
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;
    using Ardalis.Specification.EntityFrameworkCore;
    using JordiAragon.SharedKernel.Contracts.DependencyInjection;
    using JordiAragon.SharedKernel.Contracts.Repositories;
    using JordiAragon.SharedKernel.Infrastructure.EntityFramework.Context;
    using JordiAragon.SharedKernel.Infrastructure.Interfaces;

    public abstract class BaseReadRepository<TDataEntity> : RepositoryBase<TDataEntity>, ISpecificationReadRepository<TDataEntity, Guid>, IScopedDependency
        where TDataEntity : class, IDataEntity
    {
        protected BaseReadRepository(BaseBusinessModelContext readContext)
            : base(readContext)
        {
        }

        public virtual Task<TDataEntity> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
        {
            return this.GetByIdAsync<Guid>(id, cancellationToken);
        }
    }
}