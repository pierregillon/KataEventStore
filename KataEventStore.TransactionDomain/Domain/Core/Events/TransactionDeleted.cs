using KataEventStore.TransactionDomain.Domain.Core._Base;

namespace KataEventStore.TransactionDomain.Domain.Core.Events
{
    public class TransactionDeleted : IDomainEvent
    {
        public TransactionId TransactionId { get; }

        public TransactionDeleted(TransactionId transactionId)
        {
            TransactionId = transactionId;
        }
    }
}