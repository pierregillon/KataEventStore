using System;

namespace KataEventStore.TransactionPresentation.Projections {
    public class TransactionListItem
    {
        public Guid Id { get; set; }
        public decimal Amount { get; set; }
        public string Name { get; set; }
    }
}