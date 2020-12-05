using System;
using System.Linq;
using KataEventStore.Events;

namespace KataEventStore.TransactionDomain.Domain.Infrastructure {
    public static class TypeExtensions
    {
        public static bool IsDomainEvent(this Type type) => !type.IsAbstract && type.GetInterfaces().Contains(typeof(IDomainEvent));
    }
}