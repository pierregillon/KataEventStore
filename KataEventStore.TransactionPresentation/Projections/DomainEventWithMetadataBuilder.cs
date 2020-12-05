using System;
using KataEventStore.Events;

namespace KataEventStore.TransactionPresentation.Projections {
    public static class DomainEventWithMetadataBuilder
    {
        public static object WrapWithMeta(this IDomainEvent domainEvent, DomainEventMetadata metadata)
        {
            var type = typeof(DomainEventWithMetadata<>).MakeGenericType(domainEvent.GetType());
            return Activator.CreateInstance(type, domainEvent, metadata ?? new DomainEventMetadata());
        }
    }
}