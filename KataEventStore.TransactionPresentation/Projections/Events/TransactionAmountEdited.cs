using System;
using MediatR;

namespace KataEventStore.TransactionPresentation.Projections.Events
{
    public class TransactionAmountEdited : IRequest
    {
        public Guid Id { get; }
        public decimal OldAmount { get; }
        public decimal NewAmount { get; }

        public TransactionAmountEdited(Guid id, decimal oldAmount, decimal newAmount)
        {
            Id = id;
            OldAmount = oldAmount;
            NewAmount = newAmount;
        }
    }
}