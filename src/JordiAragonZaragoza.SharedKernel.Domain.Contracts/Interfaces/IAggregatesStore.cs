namespace JordiAragonZaragoza.SharedKernel.Domain.Contracts.Interfaces
{
    using System.Collections.Generic;
    using JordiAragonZaragoza.SharedKernel.Contracts.Events;

    public interface IAggregatesStore
    {
        IEnumerable<IEventsContainer<IEvent>> EventableEntities { get; }
    }
}