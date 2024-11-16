namespace JordiAragonZaragoza.SharedKernel.Infrastructure.Idempotency
{
    using System;
    using System.Linq;
    using Ardalis.GuardClauses;
    using Ardalis.Specification;

    public class IdempotentConsumerByMessageIdConsumerFullNameSpec : SingleResultSpecification<IdempotentConsumer>
    {
        public IdempotentConsumerByMessageIdConsumerFullNameSpec(Guid messageId, string consumerFullName)
        {
            Guard.Against.Default(messageId);
            ArgumentNullException.ThrowIfNullOrWhiteSpace(consumerFullName);

            this.Query
                .Where(consumer => consumer.MessageId == messageId
                                && consumer.ConsumerFullName == consumerFullName);
        }
    }
}