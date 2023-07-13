namespace JordiAragon.SharedKernel.Domain.Events.Services
{
    using System.Collections.Generic;
    using System.Threading;
    using System.Threading.Tasks;
    using JordiAragon.SharedKernel.Contracts.Events;

    public interface IEventsDispatcherService
    {
        Task DispatchAndClearEventsAsync(IEnumerable<IEventsContainer<IEvent>> eventableEntities, CancellationToken cancellationToken = default);
    }
}