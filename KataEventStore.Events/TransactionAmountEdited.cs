using System;

namespace KataEventStore.Events
{
    public class TransactionAmountEdited : IDomainEvent
    {
        public Guid AggregateId { get; }
        public decimal OldAmount { get; }
        public decimal NewAmount { get; }

        public TransactionAmountEdited(Guid aggregateId, decimal oldAmount, decimal newAmount)
        {
            AggregateId = aggregateId;
            OldAmount = oldAmount;
            NewAmount = newAmount;
        }
    }
}