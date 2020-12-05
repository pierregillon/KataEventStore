using System;

namespace KataEventStore.Events
{
    public class TransactionCreated : IDomainEvent
    {
        public Guid AggregateId { get; }
        public string Name { get; }
        public decimal Amount { get; }

        public TransactionCreated(Guid aggregateId, string name, decimal amount)
        {
            AggregateId = aggregateId;
            Name = name;
            Amount = amount;
        }
    }
}