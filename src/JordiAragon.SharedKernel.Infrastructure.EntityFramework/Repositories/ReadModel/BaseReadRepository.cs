namespace JordiAragon.SharedKernel.Infrastructure.EntityFramework.Repositories.ReadModel
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;
    using Ardalis.Specification.EntityFrameworkCore;
    using JordiAragon.SharedKernel.Application.Contracts.Interfaces;
    using JordiAragon.SharedKernel.Contracts.DependencyInjection;
    using JordiAragon.SharedKernel.Contracts.Repositories;

    public abstract class BaseReadRepository<TReadModel> : RepositoryBase<TReadModel>,  ISpecificationReadRepository<TReadModel, Guid>, IScopedDependency
        where TReadModel : class, IReadModel
    {
        protected BaseReadRepository(BaseReadContext readContext)
            : base(readContext)
        {
        }

        public virtual Task<TReadModel> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
        {
            return this.GetByIdAsync<Guid>(id, cancellationToken);
        }
    }
}