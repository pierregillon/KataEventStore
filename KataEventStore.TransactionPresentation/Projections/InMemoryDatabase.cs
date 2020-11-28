using System;
using System.Collections;
using System.Collections.Generic;

namespace KataEventStore.TransactionPresentation.Projections
{
    public class InMemoryDatabase
    {
        private readonly IDictionary<Type, IList> _data = new Dictionary<Type, IList>();

        public IList<T> Table<T>()
        {
            if (_data.TryGetValue(typeof(T), out var result)) {
                return (List<T>) result;
            }
            var value = new List<T>();
            _data.Add(typeof(T), value);
            return value;
        }
    }
}