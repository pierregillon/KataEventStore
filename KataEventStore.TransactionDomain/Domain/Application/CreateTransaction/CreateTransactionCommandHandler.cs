using System.Threading.Tasks;
using KataEventStore.TransactionDomain.Domain.Application._Base;
using KataEventStore.TransactionDomain.Domain.Core;
using KataEventStore.TransactionDomain.Domain.Core._Base;

namespace KataEventStore.TransactionDomain.Domain.Application.CreateTransaction
{
    public class CreateTransactionCommandHandler : CommandHandler<CreateTransactionCommand>
    {
        private readonly IEventStore eventStore;

        public CreateTransactionCommandHandler(IEventStore eventStore)
        {
            this.eventStore = eventStore;
        }

        protected override async Task Handle(CreateTransactionCommand request)
        {
            var transactionCreated = Transaction.New(request.Name, request.Amount);

            await eventStore.Store(transactionCreated);
        }
    }
}