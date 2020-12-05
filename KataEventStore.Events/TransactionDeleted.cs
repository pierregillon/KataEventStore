using System;

namespace KataEventStore.Events
{
    public class TransactionDeleted : TransactionDomainEvent
    {
        public TransactionDeleted(Guid aggregateId) : base(aggregateId) { }
    }
}