using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using KataEventStore.TransactionPresentation.Projections;
using MediatR;

namespace KataEventStore.TransactionPresentation.Queries.GetTransactionRenamedStatistics
{
    public class GetTransactionRenamedStatisticsQueryHandler : IRequestHandler<GetTransactionRenamedStatisticsQuery, IEnumerable<TransactionRenamedStatistic>>
    {
        private readonly InMemoryDatabase _database;

        public GetTransactionRenamedStatisticsQueryHandler(InMemoryDatabase database) => _database = database;

        public Task<IEnumerable<TransactionRenamedStatistic>> Handle(GetTransactionRenamedStatisticsQuery request, CancellationToken cancellationToken)
        {
            var result = _database.Table<TransactionRenamedStatistic>().ToList();

            return Task.FromResult((IEnumerable<TransactionRenamedStatistic>) result);
        }
    }
}