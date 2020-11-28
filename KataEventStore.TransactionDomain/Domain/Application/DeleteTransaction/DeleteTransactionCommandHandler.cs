using System.Threading.Tasks;
using KataEventStore.TransactionDomain.Domain.Application._Base;
using KataEventStore.TransactionDomain.Domain.Core;
using KataEventStore.TransactionDomain.Domain.Core._Base;

namespace KataEventStore.TransactionDomain.Domain.Application.DeleteTransaction
{
    public class DeleteTransactionCommandHandler : CommandHandler<DeleteTransactionCommand>
    {
        private readonly IEventStore eventStore;

        public DeleteTransactionCommandHandler(IEventStore eventStore)
        {
            this.eventStore = eventStore;
        }

        protected override async Task Handle(DeleteTransactionCommand request)
        {
            var transaction = Transaction.Rehydrate(await eventStore.GetAllEvents(request.TransactionId));

            var deleted = transaction.Delete();

            await this.eventStore.Store(deleted);
        }
    }
}