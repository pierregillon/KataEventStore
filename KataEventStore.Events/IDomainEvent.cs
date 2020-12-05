using System;
using MediatR;

namespace KataEventStore.Events
{
    public interface IDomainEvent : IRequest
    {
        Guid AggregateId { get; }
    }
}