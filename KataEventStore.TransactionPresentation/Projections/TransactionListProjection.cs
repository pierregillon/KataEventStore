using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using KataEventStore.Events;
using MediatR;

namespace KataEventStore.TransactionPresentation.Projections
{
    public class TransactionListProjection :
        INotificationHandler<TransactionCreated>,
        INotificationHandler<TransactionRenamed>,
        INotificationHandler<TransactionAmountEdited>,
        INotificationHandler<TransactionDeleted>
    {
        private readonly InMemoryDatabase _database;

        public TransactionListProjection(InMemoryDatabase database) => _database = database;

        public Task Handle(TransactionCreated request, CancellationToken cancellationToken)
        {
            _database.Table<TransactionListItem>().Add(new TransactionListItem {
                Id = request.AggregateId,
                Name = request.Name,
                Amount = request.Amount
            });

            return Task.CompletedTask;
        }

        public Task Handle(TransactionRenamed request, CancellationToken cancellationToken)
        {
            var item = _database.Table<TransactionListItem>().Single(x => x.Id == request.AggregateId);
            if (item != null) {
                item.Name = request.NewName;
            }

            return Task.CompletedTask;
        }

        public Task Handle(TransactionAmountEdited request, CancellationToken cancellationToken)
        {
            var item = _database.Table<TransactionListItem>().SingleOrDefault(x => x.Id == request.AggregateId);
            if (item != null) {
                item.Amount = request.NewAmount;
            }
            return Task.CompletedTask;
        }

        public Task Handle(TransactionDeleted request, CancellationToken cancellationToken)
        {
            var item = _database.Table<TransactionListItem>().SingleOrDefault(x => x.Id == request.AggregateId);
            if (item != null) {
                _database.Table<TransactionListItem>().Remove(item);
            }

            return Task.CompletedTask;
        }
    }
}