using System;
using MediatR;

namespace KataEventStore.Events
{
    public interface IDomainEvent : INotification
    {
        Guid AggregateId { get; }
    }
}