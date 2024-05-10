namespace JordiAragon.SharedKernel.Infrastructure.EntityFramework.Outbox
{
    using System;
    using JordiAragon.SharedKernel.Infrastructure.Interfaces;

    public record class OutboxMessage(
        Guid Id,
        DateTimeOffset DateOccurredOnUtc,
        string Type,
        string Content) : IDataEntity
    {
        public DateTimeOffset? DateProcessedOnUtc { get; set; }

        public string Error { get; set; }

        // This allows manage optiministic concurrency
        public byte[] Version { get; protected set; }
    }
}