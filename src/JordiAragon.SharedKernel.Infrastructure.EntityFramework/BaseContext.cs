namespace JordiAragon.SharedKernel.Infrastructure.EntityFramework
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using JordiAragon.SharedKernel.Contracts.Events;
    using JordiAragon.SharedKernel.Domain.Contracts.Interfaces;
    using JordiAragon.SharedKernel.Infrastructure.EntityFramework.Interceptors;
    using JordiAragon.SharedKernel.Infrastructure.EntityFramework.Outbox;
    using JordiAragon.SharedKernel.Infrastructure.Interfaces;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Hosting;
    using Microsoft.Extensions.Logging;
    using SmartEnum.EFCore;

    public abstract class BaseContext : DbContext, IWriteStore ////, IScopedDependency
    {
        private readonly ILoggerFactory loggerFactory;
        private readonly IHostEnvironment hostEnvironment;
        private readonly AuditableEntitySaveChangesInterceptor auditableEntitySaveChangesInterceptor;

        protected BaseContext(
            DbContextOptions options,
            ILoggerFactory loggerFactory,
            IHostEnvironment hostEnvironment,
            AuditableEntitySaveChangesInterceptor auditableEntitySaveChangesInterceptor)
            : base(options)
        {
            this.loggerFactory = loggerFactory ?? throw new ArgumentNullException(nameof(loggerFactory));
            this.hostEnvironment = hostEnvironment ?? throw new ArgumentNullException(nameof(hostEnvironment));
            this.auditableEntitySaveChangesInterceptor = auditableEntitySaveChangesInterceptor ?? throw new ArgumentNullException(nameof(hostEnvironment));
        }

        public DbSet<OutboxMessage> OutboxMessages => this.Set<OutboxMessage>();

        public IEnumerable<IEventsContainer<IEvent>> EventableEntities
            => this.ChangeTracker.Entries<IEventsContainer<IDomainEvent>>()
                            .Select(e => e.Entity)
                            .Where(entity => entity.Events.Any());

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder
                .UseLoggerFactory(this.loggerFactory)
                .EnableSensitiveDataLogging(this.hostEnvironment.EnvironmentName == "Development")
                .EnableDetailedErrors(this.hostEnvironment.EnvironmentName == "Development");

            optionsBuilder.AddInterceptors(this.auditableEntitySaveChangesInterceptor);

            base.OnConfiguring(optionsBuilder);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(InfrastructureEntityFrameworkAssemblyReference.Assembly);

            base.OnModelCreating(modelBuilder);
        }

        protected override void ConfigureConventions(ModelConfigurationBuilder configurationBuilder)
        {
            configurationBuilder.ConfigureSmartEnum();

            base.ConfigureConventions(configurationBuilder);
        }
    }
}