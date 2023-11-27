namespace JordiAragon.SharedKernel.Infrastructure.EntityFramework.Outbox
{
    using System;
    using JordiAragon.SharedKernel.Infrastructure.EntityFramework.Configuration;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;

    public class OutboxMessageConfiguration : BaseEntityTypeConfiguration<OutboxMessage, OutboxMessageId>
    {
        public override void Configure(EntityTypeBuilder<OutboxMessage> builder)
        {
            base.Configure(builder);

            builder.Property(x => x.Id)
                .HasConversion(
                    outboxMessageId => outboxMessageId.Value,
                    value => OutboxMessageId.Create(value))
                .IsRequired()
                .ValueGeneratedNever();
        }
    }
}