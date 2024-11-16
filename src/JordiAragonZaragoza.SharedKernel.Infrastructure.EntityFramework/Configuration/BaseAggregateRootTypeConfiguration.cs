namespace JordiAragonZaragoza.SharedKernel.Infrastructure.EntityFramework.Configuration
{
    using System;
    using JordiAragonZaragoza.SharedKernel.Domain.Contracts.Interfaces;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;

    public abstract class BaseAggregateRootTypeConfiguration<TAggregateRoot, TId> : BaseModelTypeConfiguration<TAggregateRoot, TId>
        where TAggregateRoot : class, IAggregateRoot<TId>
        where TId : class, IEntityId
    {
        public override void Configure(EntityTypeBuilder<TAggregateRoot> builder)
        {
            ArgumentNullException.ThrowIfNull(builder, nameof(builder));

            base.Configure(builder);

            builder.Property(aggregateRoot => aggregateRoot.Version)
                .IsRowVersion();

            builder.Property<bool>("IsDeleted");
            builder.HasQueryFilter(b => !EF.Property<bool>(b, "IsDeleted"));
        }
    }
}