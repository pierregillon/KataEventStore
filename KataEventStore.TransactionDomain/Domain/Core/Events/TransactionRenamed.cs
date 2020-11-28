using KataEventStore.TransactionDomain.Domain.Core._Base;

namespace KataEventStore.TransactionDomain.Domain.Core.Events
{
    public class TransactionRenamed : IDomainEvent
    {
        public TransactionId Id { get; }
        public string OldName { get; }
        public string NewName { get; }

        public TransactionRenamed(in TransactionId id, string oldName, string newName)
        {
            Id = id;
            OldName = oldName;
            NewName = newName;
        }
    }
}