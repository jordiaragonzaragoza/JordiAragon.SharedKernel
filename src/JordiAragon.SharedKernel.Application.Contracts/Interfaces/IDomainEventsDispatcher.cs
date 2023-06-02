namespace JordiAragon.SharedKernel.Application.Contracts.Interfaces
{
    using System.Threading;
    using System.Threading.Tasks;

    public interface IDomainEventsDispatcher
    {
        Task DispatchDomainEventsAsync(CancellationToken cancellationToken = default);
    }
}