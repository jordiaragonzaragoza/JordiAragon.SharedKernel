namespace JordiAragon.SharedKernel.Infrastructure.EntityFramework.Context
{
    using System;
    using Ardalis.GuardClauses;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Hosting;
    using Microsoft.Extensions.Logging;
    using SmartEnum.EFCore;

    public abstract class BaseContext : DbContext
    {
        private readonly ILoggerFactory loggerFactory;
        private readonly IHostEnvironment hostEnvironment;
        private bool disposed;

        protected BaseContext(
            DbContextOptions options,
            ILoggerFactory loggerFactory,
            IHostEnvironment hostEnvironment)
            : base(options)
        {
            this.loggerFactory = Guard.Against.Null(loggerFactory, nameof(loggerFactory));
            this.hostEnvironment = Guard.Against.Null(hostEnvironment, nameof(hostEnvironment));
        }

        public override void Dispose()
        {
            if (!this.disposed)
            {
                this.loggerFactory?.Dispose();

                this.disposed = true;

#pragma warning disable S3971 // "GC.SuppressFinalize" should not be called
                GC.SuppressFinalize(this);
#pragma warning restore S3971 // "GC.SuppressFinalize" should not be called
            }

            base.Dispose();
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            Guard.Against.Null(optionsBuilder, nameof(optionsBuilder));

            optionsBuilder
                .UseLoggerFactory(this.loggerFactory)
                .EnableSensitiveDataLogging(this.hostEnvironment.EnvironmentName == "Development")
                .EnableDetailedErrors(this.hostEnvironment.EnvironmentName == "Development");

            base.OnConfiguring(optionsBuilder);
        }

        protected override void ConfigureConventions(ModelConfigurationBuilder configurationBuilder)
        {
            configurationBuilder.ConfigureSmartEnum();

            base.ConfigureConventions(configurationBuilder);
        }
    }
}