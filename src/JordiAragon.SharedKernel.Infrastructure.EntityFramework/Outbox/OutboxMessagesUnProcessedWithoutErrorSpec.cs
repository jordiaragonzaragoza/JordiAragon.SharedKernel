namespace JordiAragon.SharedKernel.Infrastructure.EntityFramework.Outbox
{
    using System.Linq;
    using Ardalis.Specification;

    public class OutboxMessagesUnProcessedWithoutErrorSpec : Specification<OutboxMessage>
    {
        public OutboxMessagesUnProcessedWithoutErrorSpec()
        {
            this.Query.Where(outboxMessage => outboxMessage.DateProcessedOnUtc == null && outboxMessage.Error == null);
        }
    }
}
