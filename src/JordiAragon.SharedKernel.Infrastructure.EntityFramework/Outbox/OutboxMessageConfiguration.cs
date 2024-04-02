namespace JordiAragon.SharedKernel.Infrastructure.EntityFramework.Outbox
{
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;

    public class OutboxMessageConfiguration : IEntityTypeConfiguration<OutboxMessage>
    {
        public void Configure(EntityTypeBuilder<OutboxMessage> builder)
        {
            builder.HasKey(outboxMessage => outboxMessage.Id);
        }
    }
}