using KataEventStore.Events;
using MediatR;

namespace KataEventStore.TransactionPresentation.Projections {
    public class DomainEventWithMetadata<T> : INotification where T : IDomainEvent
    {
        public T DomainEvent { get; }

        public DomainEventMetadata Metadata { get; }

        public DomainEventWithMetadata(T domainEvent, DomainEventMetadata metadata)
        {
            Metadata = metadata;
            DomainEvent = domainEvent;
        }
    }
}