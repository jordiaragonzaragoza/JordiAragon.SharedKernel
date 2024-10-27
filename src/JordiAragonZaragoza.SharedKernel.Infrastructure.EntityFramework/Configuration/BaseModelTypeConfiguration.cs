namespace JordiAragonZaragoza.SharedKernel.Infrastructure.EntityFramework.Configuration
{
    using Ardalis.GuardClauses;
    using JordiAragonZaragoza.SharedKernel.Contracts.Model;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;

    public abstract class BaseModelTypeConfiguration<TModel, TId> : IEntityTypeConfiguration<TModel>
        where TModel : class, IBaseModel<TId>
        where TId : notnull
    {
        public virtual void Configure(EntityTypeBuilder<TModel> builder)
        {
            Guard.Against.Null(builder, nameof(builder));

            builder.HasKey(x => x.Id);
        }
    }
}