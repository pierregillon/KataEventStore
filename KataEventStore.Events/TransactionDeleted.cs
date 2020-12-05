using System;

namespace KataEventStore.Events
{
    public class TransactionDeleted : IDomainEvent
    {
        public Guid AggregateId { get; }

        public TransactionDeleted(Guid aggregateId) => AggregateId = aggregateId;
    }
}