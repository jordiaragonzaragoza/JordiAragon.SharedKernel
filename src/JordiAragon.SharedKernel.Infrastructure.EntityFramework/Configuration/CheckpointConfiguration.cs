namespace JordiAragon.SharedKernel.Infrastructure.EntityFramework.Configuration
{
    using JordiAragon.SharedKernel.Infrastructure.ProjectionCheckpoint;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;

    public class CheckpointConfiguration : IEntityTypeConfiguration<Checkpoint>
    {
        public void Configure(EntityTypeBuilder<Checkpoint> builder)
        {
            builder.HasKey(ckeckpoint => ckeckpoint.Id);
        }
    }
}