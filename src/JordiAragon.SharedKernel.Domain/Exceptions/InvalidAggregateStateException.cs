namespace JordiAragon.SharedKernel.Domain.Exceptions
{
    using System;
    using System.Runtime.Serialization;
    using JordiAragon.SharedKernel.Domain.Contracts.Interfaces;

    [Serializable]
    public sealed class InvalidAggregateStateException<TAggregate, TId> : Exception
        where TAggregate : class, IAggregateRoot<TId>
        where TId : class, IEntityId
    {
        public InvalidAggregateStateException(TAggregate aggregate, string? message = null)
                : base($"Aggregate {aggregate.GetType().Name} - {aggregate.Id} state change rejected. {message}")
        {
        }

        private InvalidAggregateStateException(SerializationInfo serializationInfo, StreamingContext streamingContext)
            : base(serializationInfo, streamingContext)
        {
        }
    }
}