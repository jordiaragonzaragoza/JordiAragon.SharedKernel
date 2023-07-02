namespace JordiAragon.SharedKernel.Domain.Events.Services
{
    using System.Collections.Generic;
    using System.Threading;
    using System.Threading.Tasks;
    using JordiAragon.SharedKernel.Domain.Contracts.Interfaces;

    public interface IDomainEventsDispatcherService
    {
        Task DispatchAndClearEventsAsync(IEnumerable<IDomainEventsContainer> eventableEntities, CancellationToken cancellationToken = default);
    }
}