using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using KataEventStore.Events;
using KataEventStore.TransactionPresentation.Projections;
using MediatR;

namespace KataEventStore.TransactionPresentation.Queries.GetTransactionRenamedStatistics
{
    public class TransactionRenamedStatisticsProjection : INotificationHandler<DomainEventWithMetadata<TransactionRenamed>>
    {
        private readonly InMemoryDatabase _database;

        public TransactionRenamedStatisticsProjection(InMemoryDatabase database) => _database = database;

        public Task Handle(DomainEventWithMetadata<TransactionRenamed> @event, CancellationToken cancellationToken)
        {
            var value = @event.Metadata.CreationDate.ToString("yyyy-MM-dd HH:mm");
            var statistic = _database.Table<TransactionRenamedStatistic>().SingleOrDefault(x => x.Key == value);
            if (statistic == null) {
                _database.Table<TransactionRenamedStatistic>().Add(new TransactionRenamedStatistic {
                    Key = value,
                    Count = 1
                });
            }
            else {
                statistic.Count++;
            }
            return Task.CompletedTask;
        }
    }
}