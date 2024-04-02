namespace JordiAragon.SharedKernel.Infrastructure.EntityFramework.Idempotency
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
            Guard.Against.NullOrWhiteSpace(consumerFullName);

            this.Query
                .Where(consumer => consumer.MessageId == messageId
                                && consumer.ConsumerFullName == consumerFullName);
        }
    }
}