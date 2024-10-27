namespace JordiAragonZaragoza.SharedKernel.Infrastructure.Idempotency
{
    using System;
    using JordiAragonZaragoza.SharedKernel.Infrastructure.Interfaces;

    public record class IdempotentConsumer(Guid Id, Guid MessageId, string ConsumerFullName) : IDataEntity;
}