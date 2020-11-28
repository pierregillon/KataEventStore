using KataEventStore.TransactionDomain.Domain.Core._Base;

namespace KataEventStore.TransactionDomain.Domain.Core.Events
{
    public class TransactionCreated : IDomainEvent
    {
        public TransactionId Id { get; }
        public string Name { get; }
        public decimal Amount { get; }

        public TransactionCreated(TransactionId id, string name, decimal amount)
        {
            Id = id;
            Name = name;
            Amount = amount;
        }
    }
}