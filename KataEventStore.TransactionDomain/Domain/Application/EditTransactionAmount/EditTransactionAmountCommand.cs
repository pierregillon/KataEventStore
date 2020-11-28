using KataEventStore.TransactionDomain.Domain.Application.DeleteTransaction;
using KataEventStore.TransactionDomain.Domain.Core;
using MediatR;

namespace KataEventStore.TransactionDomain.Domain.Application.EditTransactionAmount
{
    public class EditTransactionAmountCommand : IRequest
    {
        public TransactionId Id { get; }
        public decimal Amount { get; }

        public EditTransactionAmountCommand(TransactionId id, decimal amount)
        {
            Id = id;
            Amount = amount;
        }
    }
}
