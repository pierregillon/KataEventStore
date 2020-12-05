using System.Threading.Tasks;
using KataEventStore.TransactionDomain.Domain.Application._Base;
using KataEventStore.TransactionDomain.Domain.Core;
using KataEventStore.TransactionDomain.Domain.Core._Base;

namespace KataEventStore.TransactionDomain.Domain.Application.DeleteTransaction
{
    public class DeleteTransactionCommandHandler : CommandHandler<DeleteTransactionCommand>
    {
        private readonly IEventStore _eventStore;

        public DeleteTransactionCommandHandler(IEventStore eventStore) => _eventStore = eventStore;

        protected override async Task Handle(DeleteTransactionCommand request)
        {
            var transaction = Transaction.Rehydrate(await _eventStore.GetAllEvents(request.TransactionId));

            var events = transaction.Delete();

            await _eventStore.Store(events);
        }
    }
}