namespace JordiAragon.SharedKernel.Application.Contracts.Interfaces
{
    using System.Collections.Generic;

    public interface IApplicationEventsContainer
    {
        IEnumerable<IApplicationEvent> ApplicationEvents { get; }

        ////void ClearApplicationEvents(); // TODO: Review. Is it required?
    }
}