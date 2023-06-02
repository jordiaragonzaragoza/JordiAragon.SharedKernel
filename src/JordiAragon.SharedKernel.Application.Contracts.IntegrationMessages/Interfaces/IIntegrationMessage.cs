namespace JordiAragon.SharedKernel.Application.Contracts.IntegrationMessages.Interfaces
{
    using System;

    /// <summary>
    /// Integration Message is an event that is used to communicate with other systems outside the problem domain.
    /// The IntegrationMessage is a public event, part of the Published Language.
    /// </summary>
    public interface IIntegrationMessage
    {
        public Guid Id { get; }

        public string UserId { get; } // TODO: Review if its required to audit changes.

        public DateTime DateOccurredOnUtc { get; }

        public DateTime? DatePublishedOnUtc { get; set; }
    }
}