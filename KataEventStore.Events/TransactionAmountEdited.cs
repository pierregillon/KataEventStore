using System;

namespace KataEventStore.Events
{
    public class TransactionAmountEdited : TransactionDomainEvent
    {
        public decimal OldAmount { get; }
        public decimal NewAmount { get; }

        public TransactionAmountEdited(Guid aggregateId, decimal oldAmount, decimal newAmount) : base(aggregateId)
        {
            OldAmount = oldAmount;
            NewAmount = newAmount;
        }
    }
}