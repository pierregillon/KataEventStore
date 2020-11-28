using System;
using MediatR;

namespace KataEventStore.TransactionPresentation.Projections.Events
{
    public class TransactionCreated : IRequest
    {
        public Guid Id { get; }
        public string Name { get; }
        public decimal Amount { get; }

        public TransactionCreated(Guid id, string name, decimal amount)
        {
            Id = id;
            Name = name;
            Amount = amount;
        }
    }
}