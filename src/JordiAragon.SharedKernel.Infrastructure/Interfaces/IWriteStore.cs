namespace JordiAragon.SharedKernel.Infrastructure.Interfaces
{
    using System.Collections.Generic;
    using JordiAragon.SharedKernel.Contracts.Events;

    public interface IWriteStore
    {
        IEnumerable<IEventsContainer<IEvent>> EventableEntities { get; }
    }
}