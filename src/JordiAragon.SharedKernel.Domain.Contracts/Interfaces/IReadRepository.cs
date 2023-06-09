﻿namespace JordiAragon.SharedKernel.Domain.Contracts.Interfaces
{
    using Ardalis.Specification;

    public interface IReadRepository<T> : IReadRepositoryBase<T>
        where T : class
    {
    }
}