using System;
using System.Linq;

namespace KataEventStore.Events {
    public static class TypeExtensions
    {
        public static bool IsDomainEvent(this Type type) => !type.IsAbstract && type.GetInterfaces().Contains(typeof(IDomainEvent));
    }
}