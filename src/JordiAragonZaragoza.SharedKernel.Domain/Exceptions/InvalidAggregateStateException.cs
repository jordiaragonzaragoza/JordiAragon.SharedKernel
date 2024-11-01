namespace JordiAragonZaragoza.SharedKernel.Domain.Exceptions
{
    using System;
    using JordiAragonZaragoza.SharedKernel.Domain.Contracts.Interfaces;

    public sealed class InvalidAggregateStateException<TAggregate, TId> : Exception
        where TAggregate : class, IAggregateRoot<TId>
        where TId : class, IEntityId
    {
        public InvalidAggregateStateException()
        {
        }

        public InvalidAggregateStateException(string message)
            : base(message)
        {
        }

        public InvalidAggregateStateException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        public InvalidAggregateStateException(TAggregate aggregate, string? message = null)
                : base($"Aggregate {aggregate?.GetType().Name} - {aggregate?.Id} state change rejected. {message}")
        {
        }
    }
}