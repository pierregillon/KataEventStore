using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace KataEventStore.TransactionDomain.Domain.Core._Base
{
    public interface IEventStore
    {
        Task Store(IDomainEvent domainEvent);
        Task<IEnumerable<IDomainEvent>> GetAllEvents(Guid id);
    }
}