namespace JordiAragon.SharedKernel.Infrastructure.EntityFramework.Idempotency
{
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;

    public class IdempotentConsumerConfiguration : IEntityTypeConfiguration<IdempotentConsumer>
    {
        public void Configure(EntityTypeBuilder<IdempotentConsumer> builder)
        {
            builder.HasKey(idempotentConsumer => idempotentConsumer.Id);
        }
    }
}