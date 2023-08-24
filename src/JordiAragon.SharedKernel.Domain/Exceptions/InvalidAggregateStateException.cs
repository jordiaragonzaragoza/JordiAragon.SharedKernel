namespace JordiAragon.SharedKernel.Domain.Exceptions
{
    using System;
    using System.Runtime.Serialization;
    using JordiAragon.SharedKernel.Domain.Contracts.Interfaces;

    [Serializable]
    public class InvalidAggregateStateException : Exception
    {
        public InvalidAggregateStateException(IAggregateRoot aggregate, string message = null)
                : base($"Aggregate {aggregate.GetType().Name} state change rejected. {message}")
        {
        }

        protected InvalidAggregateStateException(SerializationInfo serializationInfo, StreamingContext streamingContext)
            : base(serializationInfo, streamingContext)
        {
        }
    }
}