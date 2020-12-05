using System.Threading.Tasks;
using FluentAsync;
using KataEventStore.TransactionDomain.Domain.Application._Base;
using KataEventStore.TransactionDomain.Domain.Core;
using KataEventStore.TransactionDomain.Domain.Core._Base;

namespace KataEventStore.TransactionDomain.Domain.Application.RenameTransaction
{
    public class RenameTransactionCommandHandler : CommandHandler<RenameTransactionCommand>
    {
        private readonly IEventStore _eventStore;

        public RenameTransactionCommandHandler(IEventStore eventStore) => _eventStore = eventStore;

        protected override async Task Handle(RenameTransactionCommand command)
        {
            await _eventStore.GetAllEvents(command.TransactionId)
                .PipeAsync(Transaction.Rehydrate)
                .PipeAsync(transaction => transaction.Rename(command.Name))
                .PipeAsync(_eventStore.Store);
        }
    }
}