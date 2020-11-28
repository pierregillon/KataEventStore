using System;
using MediatR;

namespace KataEventStore.TransactionPresentation.Projections.Events
{
    public class TransactionRenamed : IRequest
    {
        public Guid Id { get; }
        public string OldName { get; }
        public string NewName { get; }

        public TransactionRenamed(Guid id, string oldName, string newName)
        {
            Id = id;
            OldName = oldName;
            NewName = newName;
        }
    }
}