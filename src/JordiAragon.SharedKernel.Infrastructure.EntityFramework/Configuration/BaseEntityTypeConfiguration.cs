namespace JordiAragon.SharedKernel.Infrastructure.EntityFramework.Configuration
{
    using JordiAragon.SharedKernel.Domain.Entities;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;

    public abstract class BaseEntityTypeConfiguration<T, TId> : IEntityTypeConfiguration<T>
        where T : BaseEntity<TId>
    {
        public virtual void Configure(EntityTypeBuilder<T> builder)
        {
            builder.HasKey(x => x.Id);
        }
    }
}
