namespace JordiAragon.SharedKernel.Infrastructure.EntityFramework.Configuration
{
    using System;
    using Ardalis.GuardClauses;
    using JordiAragon.SharedKernel.Infrastructure.Outbox;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;

    public class OutboxMessageConfiguration : BaseModelTypeConfiguration<OutboxMessage, Guid>
    {
        public override void Configure(EntityTypeBuilder<OutboxMessage> builder)
        {
            Guard.Against.Null(builder, nameof(builder));

            builder.ToTable("__OutboxMessages");

            base.Configure(builder);

            builder.Property(outboxMessage => outboxMessage.Version)
                .IsRowVersion();
        }
    }
}