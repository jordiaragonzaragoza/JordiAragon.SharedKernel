namespace JordiAragon.SharedKernel.Infrastructure.ProjectionCheckpoint
{
    using System;
    using JordiAragon.SharedKernel.Infrastructure.Interfaces;

    public sealed record class Checkpoint(Guid Id, ulong Position) : IDataEntity;
}