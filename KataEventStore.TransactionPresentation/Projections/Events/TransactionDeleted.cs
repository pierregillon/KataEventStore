using System;
using MediatR;

namespace KataEventStore.TransactionPresentation.Projections.Events
{
    public class TransactionDeleted : IRequest
    {
        public Guid TransactionId { get; }

        public TransactionDeleted(Guid transactionId) => TransactionId = transactionId;
    }
}