using System;

namespace KataEventStore.TransactionPresentation.Persisted.Queries.ListActiveTransactions {
    public class TransactionListItem
    {
        public Guid Id { get; set; }
        public decimal Amount { get; set; }
        public string Name { get; set; }
    }
}