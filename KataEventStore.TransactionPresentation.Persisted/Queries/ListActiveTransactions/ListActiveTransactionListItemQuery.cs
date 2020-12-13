using System.Collections.Generic;
using MediatR;

namespace KataEventStore.TransactionPresentation.Persisted.Queries.ListActiveTransactions {
    public class ListActiveTransactionListItemQuery : IRequest<IEnumerable<TransactionListItem>> { }
}