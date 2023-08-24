namespace JordiAragon.SharedKernel.Domain.Contracts.Interfaces
{
    using System.Collections.Generic;

    public interface IEventSourced
    {
        public int Version { get; }

        void Load(IEnumerable<IDomainEvent> history);
    }
}