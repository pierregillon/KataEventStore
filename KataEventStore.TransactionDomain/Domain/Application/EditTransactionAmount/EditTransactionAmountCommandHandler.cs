using System.Threading.Tasks;
using KataEventStore.TransactionDomain.Domain.Application._Base;
using KataEventStore.TransactionDomain.Domain.Application.CreateTransaction;
using KataEventStore.TransactionDomain.Domain.Core;
using KataEventStore.TransactionDomain.Domain.Core._Base;

namespace KataEventStore.TransactionDomain.Domain.Application.EditTransactionAmount
{
    public class EditTransactionAmountCommandHandler : CommandHandler<EditTransactionAmountCommand>
    {
        private readonly IEventStore eventStore;

        public EditTransactionAmountCommandHandler(IEventStore eventStore)
        {
            this.eventStore = eventStore;
        }

        protected override async Task Handle(EditTransactionAmountCommand command)
        {
            var transaction = Transaction.Rehydrate(await this.eventStore.GetAllEvents(command.Id));

            var amountEdited = transaction.EditAmount(command.Amount);

            await this.eventStore.Store(amountEdited);
        }
    }
}