using MediatR;

namespace KataEventStore.TransactionDomain.Domain.Application.CreateTransaction
{
    public class CreateTransactionCommand : IRequest
    {
        public string Name { get; }
        public decimal Amount { get; }

        public CreateTransactionCommand(string name, decimal amount)
        {
            Name = name;
            Amount = amount;
        }
    }
}
