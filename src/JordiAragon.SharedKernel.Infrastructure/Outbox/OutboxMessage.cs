namespace JordiAragon.SharedKernel.Infrastructure.Outbox
{
    using System;
    using JordiAragon.SharedKernel.Contracts.Model;
    using JordiAragon.SharedKernel.Infrastructure.Interfaces;

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