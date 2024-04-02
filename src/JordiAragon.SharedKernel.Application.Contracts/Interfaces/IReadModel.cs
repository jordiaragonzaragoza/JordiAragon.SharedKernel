namespace JordiAragon.SharedKernel.Application.Contracts.Interfaces
{
    using System;
    using JordiAragon.SharedKernel.Contracts.Model;

    /// <summary>
    /// Generic abstraction for a application read model.
    /// </summary>
    public interface IReadModel : IBaseModel<Guid>
    {
    }
}