namespace JordiAragon.SharedKernel.Infrastructure.EntityFramework
{
    using System.Threading;
    using System.Threading.Tasks;
    using Ardalis.Specification.EntityFrameworkCore;
    using JordiAragon.SharedKernel.Contracts.DependencyInjection;
    using JordiAragon.SharedKernel.Domain.Contracts.Interfaces;

    public abstract class BaseRepository<TAggregate, TId, TIdType> : RepositoryBase<TAggregate>, IRepository<TAggregate, TId, TIdType>, IScopedDependency
        where TAggregate : class, IAggregateRoot<TId, TIdType>
        where TId : IEntityId<TIdType>
    {
        protected BaseRepository(BaseContext dbContext)
            : base(dbContext)
        {
        }

        public Task<TAggregate> GetByIdAsync(TId id, CancellationToken cancellationToken = default)
        {
            // TODO: Review workaround.
            return base.GetByIdAsync(id, cancellationToken);
        }
    }
}
