namespace JordiAragon.SharedKernel.Infrastructure.EntityFramework.Configuration
{
    using JordiAragon.SharedKernel.Domain.Contracts.Interfaces;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;

    public abstract class BaseAggregateRootTypeConfiguration<TAggregateRoot, TId> : BaseModelTypeConfiguration<TAggregateRoot, TId>
        where TAggregateRoot : class, IAggregateRoot<TId>
        where TId : class, IEntityId
    {
        public override void Configure(EntityTypeBuilder<TAggregateRoot> builder)
        {
            base.Configure(builder);

            builder.Property(aggregateRoot => aggregateRoot.Version)
                .IsRowVersion();
        }
    }
}
