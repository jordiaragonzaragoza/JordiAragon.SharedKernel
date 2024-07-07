namespace JordiAragon.SharedKernel.Infrastructure.ProjectionCheckpoint
{
    using System;
    using Ardalis.GuardClauses;
    using JordiAragon.SharedKernel.Application.Contracts.Interfaces;

    public sealed record class Checkpoint : IReadModel
    {
        public Checkpoint(
            Guid id,
            ulong position,
            DateTimeOffset checkpointedAtOnUtc)
        {
            this.Id = Guard.Against.Default(id, nameof(id));
            this.Position = Guard.Against.Default(position, nameof(position));
            this.CheckpointedAtOnUtc = Guard.Against.Default(checkpointedAtOnUtc, nameof(checkpointedAtOnUtc));
        }

        public Guid Id { get; }

        public ulong Position { get; set; }

        public DateTimeOffset CheckpointedAtOnUtc { get; set; }
    }
}