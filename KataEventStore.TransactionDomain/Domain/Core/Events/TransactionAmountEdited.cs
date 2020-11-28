using KataEventStore.TransactionDomain.Domain.Core._Base;

namespace KataEventStore.TransactionDomain.Domain.Core.Events
{
    public class TransactionAmountEdited : IDomainEvent
    {
        public TransactionId Id { get; }
        public decimal OldAmount { get; }
        public decimal NewAmount { get; }

        public TransactionAmountEdited(TransactionId id, decimal oldAmount, decimal newAmount)
        {
            Id = id;
            OldAmount = oldAmount;
            NewAmount = newAmount;
        }
    }
}