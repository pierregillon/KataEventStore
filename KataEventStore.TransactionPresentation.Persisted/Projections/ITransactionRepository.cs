using System.Collections.Generic;
using System.Threading.Tasks;
using KataEventStore.TransactionPresentation.Persisted.Queries.ListActiveTransactions;

namespace KataEventStore.TransactionPresentation.Persisted.Projections
{
    public interface ITransactionRepository
    {
        Task<IReadOnlyCollection<TransactionListItem>> GetTransactions();
        Task AddAsync(TransactionListItem item);
        Task Save(IEnumerable<TransactionListItem> transactions);
    }
}