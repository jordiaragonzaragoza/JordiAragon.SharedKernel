namespace JordiAragon.SharedKernel.Infrastructure.EntityFramework.Idempotency
{
    using System;
    using JordiAragon.SharedKernel.Infrastructure.Interfaces;

    public record class IdempotentConsumer(Guid Id, Guid MessageId, string ConsumerFullName) : IDataEntity;
}