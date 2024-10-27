namespace JordiAragonZaragoza.SharedKernel.Domain.Contracts.Interfaces
{
    using System;

    public interface IDateTime
    {
        public DateTimeOffset UtcNow { get; }
    }
}