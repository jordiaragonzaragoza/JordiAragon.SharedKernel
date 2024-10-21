namespace JordiAragon.SharedKernel.Application.Contracts.Interfaces
{
    using System.Threading;
    using System.Threading.Tasks;
    using JordiAragon.SharedKernel.Domain.Contracts.Interfaces;

    public interface IEventStore : IAggregatesStore, IUnitOfWork
    {
        void AppendChanges<TAggregate, TId>(TAggregate aggregate)
            where TAggregate : class, IEventSourcedAggregateRoot<TId>
            where TId : class, IEntityId;

        Task<TAggregate?> LoadAggregateAsync<TAggregate, TId>(TId aggregateId, CancellationToken cancellationToken = default)
            where TAggregate : class, IEventSourcedAggregateRoot<TId>
            where TId : class, IEntityId;

        Task SaveChangesAsync(CancellationToken cancellationToken = default);
    }
}