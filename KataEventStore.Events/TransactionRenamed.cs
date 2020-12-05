using System;

namespace KataEventStore.Events
{
    public class TransactionRenamed : TransactionDomainEvent
    {
        public string OldName { get; }
        public string NewName { get; }

        public TransactionRenamed(Guid aggregateId, string oldName, string newName) : base(aggregateId)
        {
            OldName = oldName;
            NewName = newName;
        }
    }
}