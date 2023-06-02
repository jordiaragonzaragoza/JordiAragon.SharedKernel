namespace JordiAragon.SharedKernel.Contracts.Outbox
{
    using System.Threading;
    using System.Threading.Tasks;
    using JordiAragon.SharedKernel.Contracts.Events;

    public interface IOutboxService
    {
        Task AddMessageAsync(IEventNotification<IEvent> eventNotification, CancellationToken cancellationToken = default);
    }
}