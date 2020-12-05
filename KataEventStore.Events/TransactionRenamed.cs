using System;

namespace KataEventStore.Events
{
    public class TransactionRenamed : IDomainEvent
    {
        public Guid AggregateId { get; }
        public string OldName { get; }
        public string NewName { get; }

        public TransactionRenamed(Guid aggregateId, string oldName, string newName)
        {
            AggregateId = aggregateId;
            OldName = oldName;
            NewName = newName;
        }
    }
}