namespace JordiAragon.SharedKernel.Infrastructure.Interfaces
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using JordiAragon.SharedKernel.Contracts.Events;

    public interface IAggregatesStore
    {
        IEnumerable<IEventsContainer<IEvent>> EventableEntities { get; }

        void BeginTransaction();

        Task CommitTransactionAsync();

        void RollbackTransaction();
    }
}