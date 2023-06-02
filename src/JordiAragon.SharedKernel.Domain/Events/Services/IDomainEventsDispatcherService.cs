namespace JordiAragon.SharedKernel.Domain.Events.Services
{
    using System.Collections.Generic;
    using System.Security.Cryptography;
    using System.Threading;
    using System.Threading.Tasks;
    using JordiAragon.SharedKernel.Domain;
    using JordiAragon.SharedKernel.Domain.Entities;

    public interface IDomainEventsDispatcherService
    {
        ////Task DispatchAndClearEventsAsync(IEnumerable<BaseEventableEntity> entitiesWithEvents, CancellationToken cancellationToken = default);

        Task DispatchAndClearEventsAsync(IEnumerable<dynamic> eventableEntities, CancellationToken cancellationToken = default);
    }
}