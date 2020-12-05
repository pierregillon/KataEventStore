using System;
using System.Collections.Generic;
using System.Linq;

namespace KataEventStore.Events
{
    public class ReflectionDomainEventTypeLocator : IDomainEventTypeLocator
    {
        private static readonly IDictionary<string, Type> Types = typeof(IDomainEvent)
            .Assembly
            .GetTypes()
            .Where(x => x.IsDomainEvent())
            .ToDictionary(x => x.Name);

        public bool TryGetValue(string typeName, out Type domainEventType) => Types.TryGetValue(typeName, out domainEventType);
    }
}