namespace JordiAragonZaragoza.SharedKernel.Application.Contracts.Interfaces
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;

    public interface IIdempotencyService
    {
        Task<bool> IsProcessedAsync(Guid messageId, string consumerFullName, CancellationToken cancellationToken);

        Task MarkAsProcessedAsync(Guid messageId, string consumerFullName, CancellationToken cancellationToken);
    }
}