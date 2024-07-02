namespace JordiAragon.SharedKernel.Infrastructure.EntityFramework.Context
{
    using Ardalis.GuardClauses;
    using JordiAragon.SharedKernel.Infrastructure.EntityFramework.Configuration;
    using JordiAragon.SharedKernel.Infrastructure.EntityFramework.Interceptors;
    using JordiAragon.SharedKernel.Infrastructure.Idempotency;
    using JordiAragon.SharedKernel.Infrastructure.Outbox;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Hosting;
    using Microsoft.Extensions.Logging;

    public abstract class BaseBusinessModelContext : BaseContext
    {
        private readonly SoftDeleteEntitySaveChangesInterceptor softDeleteEntitySaveChangesInterceptor;

        protected BaseBusinessModelContext(
            DbContextOptions options,
            ILoggerFactory loggerFactory,
            IHostEnvironment hostEnvironment,
            SoftDeleteEntitySaveChangesInterceptor softDeleteEntitySaveChangesInterceptor)
            : base(options, loggerFactory, hostEnvironment)
        {
            this.softDeleteEntitySaveChangesInterceptor = Guard.Against.Null(softDeleteEntitySaveChangesInterceptor, nameof(softDeleteEntitySaveChangesInterceptor));
        }

        public DbSet<OutboxMessage> OutboxMessages => this.Set<OutboxMessage>();

        public DbSet<IdempotentConsumer> IdempotentConsumers => this.Set<IdempotentConsumer>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new OutboxMessageConfiguration());
            modelBuilder.ApplyConfiguration(new IdempotentConsumerConfiguration());

            base.OnModelCreating(modelBuilder);
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.AddInterceptors(this.softDeleteEntitySaveChangesInterceptor);

            base.OnConfiguring(optionsBuilder);
        }
    }
}