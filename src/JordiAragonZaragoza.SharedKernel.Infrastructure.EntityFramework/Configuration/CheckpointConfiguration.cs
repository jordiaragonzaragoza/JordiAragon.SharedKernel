namespace JordiAragonZaragoza.SharedKernel.Infrastructure.EntityFramework.Configuration
{
    using Ardalis.GuardClauses;
    using JordiAragonZaragoza.SharedKernel.Infrastructure.ProjectionCheckpoint;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;

    public class CheckpointConfiguration : IEntityTypeConfiguration<Checkpoint>
    {
        public void Configure(EntityTypeBuilder<Checkpoint> builder)
        {
            Guard.Against.Null(builder, nameof(builder));

            builder.ToTable("__Checkpoints");

            builder.HasKey(ckeckpoint => ckeckpoint.Id);
        }
    }
}