using System;
using System.Collections.Generic;

namespace KataEventStore.TransactionPresentation.Projections {
    public class InMemoryDatabase
    {
        private readonly IDictionary<Type, IList<object>> _data = new Dictionary<Type, IList<object>>();

        public IList<T> Table<T>()
        {
            if (!_data.TryGetValue(typeof(T), out var result)) {
                result = new List<object>();
                _data.Add(typeof(T), result);
            }
            return (IList<T>) result;
        }
    }
}