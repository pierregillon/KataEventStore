using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using KataEventStore.TransactionPresentation.Persisted.Projections;
using MediatR;

namespace KataEventStore.TransactionPresentation.Persisted.Queries.ListActiveTransactions
{
    public class ListActiveTransactionListItemQueryHandler : IRequestHandler<ListActiveTransactionListItemQuery, IEnumerable<TransactionListItem>>
    {
        private readonly ITransactionRepository _transactionRepository;

        public ListActiveTransactionListItemQueryHandler(ITransactionRepository transactionRepository) => _transactionRepository = transactionRepository;

        public async Task<IEnumerable<TransactionListItem>> Handle(ListActiveTransactionListItemQuery request, CancellationToken cancellationToken) 
            => await _transactionRepository.GetTransactions();
    }
}