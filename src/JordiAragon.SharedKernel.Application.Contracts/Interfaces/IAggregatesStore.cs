namespace JordiAragon.SharedKernel.Application.Contracts.Interfaces
{
    using System.Collections.Generic;
    using JordiAragon.SharedKernel.Contracts.Events;

    public interface IAggregatesStore
    {
        IEnumerable<IEventsContainer<IEvent>> EventableEntities { get; }
    }
}