using System.Collections.Generic;
using KataEventStore.TransactionPresentation.Projections;
using MediatR;

namespace KataEventStore.TransactionPresentation.Queries.ListActiveTransactions {
    public class ListActiveTransactionListItemQuery : IRequest<IEnumerable<TransactionListItem>> { }
}