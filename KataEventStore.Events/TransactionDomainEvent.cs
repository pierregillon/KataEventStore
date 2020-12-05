using System;

namespace KataEventStore.Events
{
    public abstract class TransactionDomainEvent : IDomainEvent
    {
        public Guid AggregateId { get; }

        protected TransactionDomainEvent(Guid aggregateId) => AggregateId = aggregateId;
    }
}