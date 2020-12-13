using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using FluentAsync;
using KataEventStore.Events;
using KataEventStore.TransactionPresentation.Persisted.Projections;
using MediatR;

namespace KataEventStore.TransactionPresentation.Persisted.Queries.ListActiveTransactions
{
    public class TransactionListProjection :
        INotificationHandler<TransactionCreated>,
        INotificationHandler<TransactionRenamed>,
        INotificationHandler<TransactionAmountEdited>,
        INotificationHandler<TransactionDeleted>
    {
        private readonly ITransactionRepository _transactionRepository;

        public TransactionListProjection(ITransactionRepository transactionRepository) => _transactionRepository = transactionRepository;

        public async Task Handle(TransactionCreated @event, CancellationToken cancellationToken)
        {
            var transactions = await _transactionRepository.GetTransactions();
            var item = transactions.SingleOrDefault(x => x.Id == @event.AggregateId);
            if (item == null) {
                await _transactionRepository.AddAsync(new TransactionListItem {
                    Id = @event.AggregateId,
                    Name = @event.Name,
                    Amount = @event.Amount
                });
            }
        }

        public async Task Handle(TransactionRenamed @event, CancellationToken cancellationToken)
        {
            var transactions = await _transactionRepository.GetTransactions();
            var item = transactions.SingleOrDefault(x => x.Id == @event.AggregateId);
            if (item != null) {
                item.Name = @event.NewName;
                await _transactionRepository.Save(transactions);
            }
        }

        public async Task Handle(TransactionAmountEdited @event, CancellationToken cancellationToken)
        {
            var transactions = await _transactionRepository.GetTransactions();
            var item = transactions.SingleOrDefault(x => x.Id == @event.AggregateId);
            if (item != null) {
                item.Amount = @event.NewAmount;
                await _transactionRepository.Save(transactions);
            }
        }

        public async Task Handle(TransactionDeleted @event, CancellationToken cancellationToken)
        {
            var transactions = await _transactionRepository.GetTransactions().PipeAsync(x => x.ToList());
            var item = transactions.SingleOrDefault(x => x.Id == @event.AggregateId);
            if (item != null) {
                transactions.Remove(item);
                await _transactionRepository.Save(transactions);
            }
        }
    }
}