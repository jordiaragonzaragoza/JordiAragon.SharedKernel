namespace JordiAragon.SharedKernel.Application.Contracts.Interfaces
{
    using System;

    public interface IIdGenerator
    {
        Guid Create();
    }
}