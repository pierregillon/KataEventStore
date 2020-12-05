using System;

namespace KataEventStore.Events
{
    public interface IDomainEventTypeLocator
    {
        bool TryGetValue(string typeName, out Type domainEventType);
    }
}