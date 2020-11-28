using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using EventStore.ClientAPI;
using KataEventStore.TransactionDomain.Domain.Core._Base;

namespace KataEventStore.TransactionDomain.Domain.Infrastructure
{
    public class EventStoreOrg : IEventStore
    {
        private readonly IEventStoreConnection eventStoreConnection;

        public EventStoreOrg(/*IEventStoreConnection eventStoreConnection*/)
        {
            this.eventStoreConnection = eventStoreConnection;
        }

        public Task Store(IDomainEvent domainEvent)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<IDomainEvent>> GetAllEvents(Guid id)
        {
            throw new NotImplementedException();
        }
    }
}
