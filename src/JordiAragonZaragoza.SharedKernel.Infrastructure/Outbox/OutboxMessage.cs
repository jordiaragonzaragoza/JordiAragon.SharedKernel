namespace JordiAragonZaragoza.SharedKernel.Infrastructure.Outbox
{
    using System;
    using JordiAragonZaragoza.SharedKernel.Contracts.Model;
    using JordiAragonZaragoza.SharedKernel.Infrastructure.Interfaces;

    public record class OutboxMessage(
        Guid Id,
        DateTimeOffset DateOccurredOnUtc,
        string Type,
        string Content) : IDataEntity, IOptimisticLockable
    {
        public DateTimeOffset? DateProcessedOnUtc { get; set; }

        public string Error { get; set; } = string.Empty;

        public uint Version { get; protected set; }
    }
}