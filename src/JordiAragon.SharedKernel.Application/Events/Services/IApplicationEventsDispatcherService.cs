namespace JordiAragon.SharedKernel.Application.Events.Services
{
    using System.Collections.Generic;
    using System.Threading;
    using System.Threading.Tasks;
    using JordiAragon.SharedKernel.Application.Contracts.Interfaces;

    public interface IApplicationEventsDispatcherService
    {
        Task DispatchEventsAsync(IEnumerable<IApplicationEvent> applicationEvents, CancellationToken cancellationToken = default);
    }
}