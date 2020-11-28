using KataEventStore.TransactionDomain.Domain.Application.DeleteTransaction;
using KataEventStore.TransactionDomain.Domain.Core;
using MediatR;

namespace KataEventStore.TransactionDomain.Domain.Application.RenameTransaction
{
    public class RenameTransactionCommand : IRequest
    {
        public TransactionId TransactionId { get; }
        public string Name { get; }

        public RenameTransactionCommand(TransactionId transactionId, string name)
        {
            TransactionId = transactionId;
            Name = name;
        }
    }
}
