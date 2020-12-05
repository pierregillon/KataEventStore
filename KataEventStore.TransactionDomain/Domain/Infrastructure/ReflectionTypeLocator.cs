﻿using System;
using System.Collections.Generic;
using System.Linq;
using KataEventStore.Events;

namespace KataEventStore.TransactionDomain.Domain.Infrastructure
{
    public class ReflectionTypeLocator : ITypeLocator
    {
        private static readonly IDictionary<string, Type> Types = typeof(IDomainEvent)
            .Assembly
            .GetTypes()
            .Where(x => x.IsDomainEvent())
            .ToDictionary(x => x.Name);

        public Type FindEventType(string typeName)
        {
            if (Types.TryGetValue(typeName, out var type)) {
                return type;
            }
            return null;
        }
    }
}