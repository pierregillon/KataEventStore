using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using FluentAsync;
using KataEventStore.TransactionPresentation.Projections;
using MediatR;

namespace KataEventStore.TransactionPresentation.Queries.ListActiveTransactions {
    public class ListActiveTransactionListItemQueryHandler : IRequestHandler<ListActiveTransactionListItemQuery, IEnumerable<TransactionListItem>>
    {
        private readonly InMemoryDatabase _database;

        public ListActiveTransactionListItemQueryHandler(InMemoryDatabase database) => _database = database;

        public Task<IEnumerable<TransactionListItem>> Handle(ListActiveTransactionListItemQuery request, CancellationToken cancellationToken)
            => _database
                .Table<TransactionListItem>()
                .ToList()
                .Pipe(x => (IEnumerable<TransactionListItem>) x)
                .Pipe(Task.FromResult);
    }
}