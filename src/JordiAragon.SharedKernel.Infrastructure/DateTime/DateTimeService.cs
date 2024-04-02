namespace JordiAragon.SharedKernel.Infrastructure.DateTime
{
    using System;
    using JordiAragon.SharedKernel.Contracts.DependencyInjection;
    using JordiAragon.SharedKernel.Domain.Contracts.Interfaces;

    public class DateTimeService : IDateTime, ISingletonDependency
    {
        public DateTimeOffset UtcNow => DateTimeOffset.UtcNow;
    }
}