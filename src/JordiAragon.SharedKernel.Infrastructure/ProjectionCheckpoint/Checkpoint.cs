namespace JordiAragon.SharedKernel.Infrastructure.ProjectionCheckpoint
{
    using System;
    using JordiAragon.SharedKernel.Application.Contracts.Interfaces;

    public sealed record class Checkpoint(Guid Id, ulong Position, DateTimeOffset CheckpointedAtOnUtc) : IReadModel;
}