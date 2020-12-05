using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using KataEventStore.Events;

namespace KataEventStore.TransactionDomain.Domain.Core._Base
{
    public interface IEventStore
    {
        Task Store(IDomainEvent domainEvent);
        Task Store(IEnumerable<IDomainEvent> events);

        Task<IEnumerable<IDomainEvent>> GetAllEvents(Guid aggregateId);
    }
}