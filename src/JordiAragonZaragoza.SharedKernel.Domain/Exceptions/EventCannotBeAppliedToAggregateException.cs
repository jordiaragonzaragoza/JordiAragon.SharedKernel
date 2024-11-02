namespace JordiAragonZaragoza.SharedKernel.Domain.Exceptions
{
    using System;
    using JordiAragonZaragoza.SharedKernel.Domain.Contracts.Interfaces;

    public sealed class EventCannotBeAppliedToAggregateException<TAggregate, TId> : Exception
        where TAggregate : class, IAggregateRoot<TId>
        where TId : class, IEntityId
    {
        public EventCannotBeAppliedToAggregateException()
        {
        }

        public EventCannotBeAppliedToAggregateException(string message)
            : base(message)
        {
        }

        public EventCannotBeAppliedToAggregateException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        public EventCannotBeAppliedToAggregateException(TAggregate aggregate, IDomainEvent domainEvent)
                : base($"Domain Event {domainEvent?.GetType().Name} - {domainEvent?.Id} can not be applied to current aggregate {aggregate?.GetType().Name} - {aggregate?.Id}")
        {
        }
    }
}