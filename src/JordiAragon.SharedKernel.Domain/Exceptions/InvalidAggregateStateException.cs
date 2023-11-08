namespace JordiAragon.SharedKernel.Domain.Exceptions
{
    using System;
    using System.Runtime.Serialization;
    using JordiAragon.SharedKernel.Domain.Contracts.Interfaces;

    [Serializable]
    public sealed class InvalidAggregateStateException<T, TId> : Exception
        where T : class, IAggregateRoot<TId>
    {
        public InvalidAggregateStateException(T aggregate, string message = null)
                : base($"Aggregate {aggregate.GetType().Name} state change rejected. {message}")
        {
        }

        private InvalidAggregateStateException(SerializationInfo serializationInfo, StreamingContext streamingContext)
            : base(serializationInfo, streamingContext)
        {
        }
    }
}