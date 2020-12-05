using System.Threading.Tasks;
using KataEventStore.TransactionDomain.Domain.Application._Base;
using KataEventStore.TransactionDomain.Domain.Core;
using KataEventStore.TransactionDomain.Domain.Core._Base;

namespace KataEventStore.TransactionDomain.Domain.Application.EditTransactionAmount
{
    public class EditTransactionAmountCommandHandler : CommandHandler<EditTransactionAmountCommand>
    {
        private readonly IEventStore _eventStore;

        public EditTransactionAmountCommandHandler(IEventStore eventStore) => _eventStore = eventStore;

        protected override async Task Handle(EditTransactionAmountCommand command)
        {
            var transaction = Transaction.Rehydrate(await _eventStore.GetAllEvents(command.Id));

            var events = transaction.EditAmount(command.Amount);

            await _eventStore.Store(events);
        }
    }
}