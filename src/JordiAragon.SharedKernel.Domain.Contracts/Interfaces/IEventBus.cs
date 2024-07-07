namespace JordiAragon.SharedKernel.Domain.Contracts.Interfaces
{
    using System.Threading;
    using System.Threading.Tasks;
    using JordiAragon.SharedKernel.Contracts.Events;

    public interface IEventBus
    {
        Task PublishAsync(IEvent @event, CancellationToken cancellationToken = default);

        Task PublishAsync(IEventNotification eventNotification, CancellationToken cancellationToken = default);
    }
}