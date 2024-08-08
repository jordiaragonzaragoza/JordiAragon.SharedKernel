namespace JordiAragon.SharedKernel.Infrastructure.EntityFramework.Configuration
{
    using System;
    using JordiAragon.SharedKernel.Infrastructure.Idempotency;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;

    public class IdempotentConsumerConfiguration : BaseModelTypeConfiguration<IdempotentConsumer, Guid>
    {
        public override void Configure(EntityTypeBuilder<IdempotentConsumer> builder)
        {
            builder.ToTable("__IdempotentConsumers");

            base.Configure(builder);
        }
    }
}