namespace JordiAragon.SharedKernel.Domain.Contracts.Interfaces
{
    using System.Collections.Generic;

    public interface IDomainEventsContainer
    {
        IEnumerable<IDomainEvent> DomainEvents { get; }

        void ClearDomainEvents();
    }
}