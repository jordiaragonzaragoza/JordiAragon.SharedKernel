namespace JordiAragon.SharedKernel.Domain.Contracts.Interfaces
{
    using System.Collections.Generic;
    using JordiAragon.SharedKernel.Contracts.Events;

    public interface IAggregatesStore
    {
        IEnumerable<IEventsContainer<IEvent>> EventableEntities { get; }
    }
}