namespace JordiAragon.SharedKernel.Infrastructure.EntityFramework.Configuration
{
    using System;
    using JordiAragon.SharedKernel.Infrastructure.Idempotency;

    public class IdempotentConsumerConfiguration : BaseModelTypeConfiguration<IdempotentConsumer, Guid>
    {
    }
}