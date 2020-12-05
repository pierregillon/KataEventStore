using System;

namespace KataEventStore.Events
{
    public class TransactionCreated : TransactionDomainEvent
    {
        public string Name { get; }
        public decimal Amount { get; }

        public TransactionCreated(Guid aggregateId, string name, decimal amount) : base(aggregateId)
        {
            Name = name;
            Amount = amount;
        }
    }
}