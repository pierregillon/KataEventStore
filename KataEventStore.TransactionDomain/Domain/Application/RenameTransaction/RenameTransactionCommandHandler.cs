using System.Threading.Tasks;
using KataEventStore.TransactionDomain.Domain.Application._Base;
using KataEventStore.TransactionDomain.Domain.Core;
using KataEventStore.TransactionDomain.Domain.Core._Base;

namespace KataEventStore.TransactionDomain.Domain.Application.RenameTransaction
{
    public class RenameTransactionCommandHandler : CommandHandler<RenameTransactionCommand>
    {
        private readonly IEventStore eventStore;

        public RenameTransactionCommandHandler(IEventStore eventStore)
        {
            this.eventStore = eventStore;
        }

        protected override async Task Handle(RenameTransactionCommand command)
        {
            var transaction = Transaction.Rehydrate(await this.eventStore.GetAllEvents(command.TransactionId));

            var renamed = transaction.Rename(command.Name);

            await this.eventStore.Store(renamed);
        }
    }
}