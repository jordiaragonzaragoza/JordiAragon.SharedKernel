namespace JordiAragon.SharedKernel.Domain.Contracts.Interfaces
{
    using System;

    public interface IDateTime
    {
        public DateTime UtcNow { get; }
    }
}