namespace JordiAragon.SharedKernel.Infrastructure.EntityFramework
{
    using Ardalis.GuardClauses;
    using JordiAragon.SharedKernel.Infrastructure.EntityFramework.Interceptors;
    using JordiAragon.SharedKernel.Infrastructure.EntityFramework.Outbox;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Hosting;
    using Microsoft.Extensions.Logging;

    public abstract class BaseWriteContext : BaseReadContext
    {
        private readonly AuditableEntitySaveChangesInterceptor auditableEntitySaveChangesInterceptor;

        protected BaseWriteContext(
            DbContextOptions options,
            ILoggerFactory loggerFactory,
            IHostEnvironment hostEnvironment,
            AuditableEntitySaveChangesInterceptor auditableEntitySaveChangesInterceptor)
            : base(options, loggerFactory, hostEnvironment)
        {
            this.auditableEntitySaveChangesInterceptor = Guard.Against.Null(auditableEntitySaveChangesInterceptor, nameof(auditableEntitySaveChangesInterceptor));
        }

        public DbSet<OutboxMessage> OutboxMessages => this.Set<OutboxMessage>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new OutboxMessageConfiguration());

            base.OnModelCreating(modelBuilder);
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.AddInterceptors(this.auditableEntitySaveChangesInterceptor);

            base.OnConfiguring(optionsBuilder);
        }
    }
}