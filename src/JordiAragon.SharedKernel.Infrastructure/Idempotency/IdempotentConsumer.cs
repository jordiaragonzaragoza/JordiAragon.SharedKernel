namespace JordiAragon.SharedKernel.Infrastructure.Idempotency
{
    using System;
    using JordiAragon.SharedKernel.Infrastructure.Interfaces;

    public record class IdempotentConsumer(Guid Id, Guid MessageId, string ConsumerFullName) : IDataEntity;
}