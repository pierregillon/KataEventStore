using KataEventStore.TransactionDomain.Domain.Core;

namespace KataEventStore.TransactionPresentation.Projections {
    public class TransactionListItem
    {
        public TransactionId Id { get; set; }
        public decimal Amount { get; set; }
        public string Name { get; set; }
    }
}