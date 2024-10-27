namespace JordiAragonZaragoza.SharedKernel.Infrastructure.DateTime
{
    using System;
    using JordiAragonZaragoza.SharedKernel.Contracts.DependencyInjection;
    using JordiAragonZaragoza.SharedKernel.Domain.Contracts.Interfaces;

    public class DateTimeService : IDateTime, ISingletonDependency
    {
        public DateTimeOffset UtcNow => DateTimeOffset.UtcNow;
    }
}