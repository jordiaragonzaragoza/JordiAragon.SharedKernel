namespace JordiAragon.SharedKernel.Domain.Contracts.Interfaces
{
    using System.Collections.Generic;
    using System.Threading;
    using System.Threading.Tasks;

    public interface IRepository<TAggregate, TId> : IReadRepository<TAggregate, TId>
        where TAggregate : class, IAggregateRoot<TId>
        where TId : class, IEntityId
    {
        Task<TAggregate> AddAsync(TAggregate aggregate, CancellationToken cancellationToken = default);

        Task<IEnumerable<TAggregate>> AddRangeAsync(IEnumerable<TAggregate> entities, CancellationToken cancellationToken = default);

        Task UpdateAsync(TAggregate aggregate, CancellationToken cancellationToken = default);

        Task UpdateRangeAsync(IEnumerable<TAggregate> entities, CancellationToken cancellationToken = default);

        Task DeleteAsync(TAggregate aggregate, CancellationToken cancellationToken = default);

        Task DeleteRangeAsync(IEnumerable<TAggregate> aggregates, CancellationToken cancellationToken = default);
    }
}
