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

        public Task Handle(TransactionCreated @event, CancellationToken cancellationToken)
        {
            _database.Table<TransactionListItem>().Add(new TransactionListItem {
                Id = @event.AggregateId,
                Name = @event.Name,
                Amount = @event.Amount
            });

            return Task.CompletedTask;
        }

        public Task Handle(TransactionRenamed @event, CancellationToken cancellationToken)
        {
            var item = _database.Table<TransactionListItem>().Single(x => x.Id == @event.AggregateId);
            if (item != null) {
                item.Name = @event.NewName;
            }

            return Task.CompletedTask;
        }

        public Task Handle(TransactionAmountEdited @event, CancellationToken cancellationToken)
        {
            var item = _database.Table<TransactionListItem>().SingleOrDefault(x => x.Id == @event.AggregateId);
            if (item != null) {
                item.Amount = @event.NewAmount;
            }
            return Task.CompletedTask;
        }

        public Task Handle(TransactionDeleted @event, CancellationToken cancellationToken)
        {
            var item = _database.Table<TransactionListItem>().SingleOrDefault(x => x.Id == @event.AggregateId);
            if (item != null) {
                _database.Table<TransactionListItem>().Remove(item);
            }

            return Task.CompletedTask;
        }
    }
}