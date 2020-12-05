using System;

namespace KataEventStore.TransactionDomain.Domain.Infrastructure {
    public interface ITypeLocator {
        Type FindEventType(string eventType);
    }
}