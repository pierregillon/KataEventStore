using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using KataEventStore.Events;
using MediatR;

namespace KataEventStore.TransactionPresentation.Projections
{
    public class TransactionListProjection :
        IRequestHandler<TransactionCreated>,
        IRequestHandler<TransactionRenamed>,
        IRequestHandler<TransactionAmountEdited>,
        IRequestHandler<TransactionDeleted>
    {
        private readonly InMemoryDatabase _database;

        public TransactionListProjection(InMemoryDatabase database) => _database = database;

        public Task<Unit> Handle(TransactionCreated request, CancellationToken cancellationToken)
        {
            _database.Table<TransactionListItem>().Add(new TransactionListItem {
                Id = request.AggregateId,
                Name = request.Name,
                Amount = request.Amount
            });

            return Task.FromResult(Unit.Value);
        }

        public Task<Unit> Handle(TransactionRenamed request, CancellationToken cancellationToken)
        {
            var item = _database.Table<TransactionListItem>().Single(x => x.Id == request.AggregateId);

            item.Name = request.NewName;

            return Task.FromResult(Unit.Value);
        }

        public Task<Unit> Handle(TransactionAmountEdited request, CancellationToken cancellationToken)
        {
            var item = _database.Table<TransactionListItem>().Single(x => x.Id == request.AggregateId);

            item.Amount = request.NewAmount;

            return Task.FromResult(Unit.Value);
        }

        public Task<Unit> Handle(TransactionDeleted request, CancellationToken cancellationToken)
        {
            var item = _database.Table<TransactionListItem>().Single(x => x.Id == request.AggregateId);

            _database.Table<TransactionListItem>().Remove(item);

            return Task.FromResult(Unit.Value);
        }
    }
}