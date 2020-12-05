using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using KataEventStore.TransactionPresentation.Projections;
using MediatR;

namespace KataEventStore.TransactionPresentation.Queries.GetTransactionRenamedStatistics
{
    public class GetTransactionRenamedStatisticsQuery : IRequest<IEnumerable<TransactionRenamedStatistic>>
    {
    }
}
