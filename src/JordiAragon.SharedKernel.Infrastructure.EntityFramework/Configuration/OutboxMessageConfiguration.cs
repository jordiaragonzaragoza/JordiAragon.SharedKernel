namespace JordiAragon.SharedKernel.Infrastructure.EntityFramework.Configuration
{
    using System;
    using JordiAragon.SharedKernel.Infrastructure.Outbox;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;

    public class OutboxMessageConfiguration : BaseModelTypeConfiguration<OutboxMessage, Guid>
    {
        public override void Configure(EntityTypeBuilder<OutboxMessage> builder)
        {
            base.Configure(builder);

            builder.Property(outboxMessage => outboxMessage.Version)
                .IsRowVersion();
        }
    }
}