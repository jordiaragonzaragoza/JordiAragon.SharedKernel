namespace JordiAragon.SharedKernel.Infrastructure.IIdGenerator.VoloAbpGuids
{
    using System;
    using Ardalis.GuardClauses;
    using JordiAragon.SharedKernel.Application.Contracts.Interfaces;
    using JordiAragon.SharedKernel.Contracts.DependencyInjection;
    using Volo.Abp.Guids;

    public class VoloAbpGuidsIdGenerator : IIdGenerator, ITransientDependency
    {
        private readonly IGuidGenerator guidGenerator;

        public VoloAbpGuidsIdGenerator(IGuidGenerator guidGenerator)
        {
            this.guidGenerator = Guard.Against.Null(guidGenerator, nameof(guidGenerator));
        }

        public Guid Create()
            => this.guidGenerator.Create();
    }
}