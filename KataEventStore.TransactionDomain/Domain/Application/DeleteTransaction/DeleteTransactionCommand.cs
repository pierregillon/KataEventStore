using KataEventStore.TransactionDomain.Domain.Core;
using MediatR;

namespace KataEventStore.TransactionDomain.Domain.Application.DeleteTransaction
{
    public class DeleteTransactionCommand : IRequest
    {
        public DeleteTransactionCommand(TransactionId transactionId)
        {
            TransactionId = transactionId;
        }

        public TransactionId TransactionId { get; }
    }
}