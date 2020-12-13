using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using EventStore.ClientAPI;
using FluentAsync;
using KataEventStore.Events;
using KataEventStore.TransactionDomain.Domain.Core._Base;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace KataEventStore.TransactionDomain.Domain.Infrastructure
{
    public class EventStoreOrg : IEventStore
    {
        private const int EVENT_COUNT = 200;

        private readonly IDomainEventTypeLocator _domainEventTypeLocator;
        private readonly IEventStoreConnection _connection;
        private readonly ILogger<EventStoreOrg> _logger;

        public EventStoreOrg(IDomainEventTypeLocator domainEventTypeLocator, IEventStoreConnection connection, ILogger<EventStoreOrg> logger)
        {
            _domainEventTypeLocator = domainEventTypeLocator;
            _connection = connection;
            _logger = logger;
        }

        // ----- Public methods

        public async Task Store(IEnumerable<IDomainEvent> events)
        {
            foreach (var domainEvent in events) {
                await Store(domainEvent);
            }
        }

        public async Task Store(IDomainEvent @event)
        {
            var eventData = new EventData(
                Guid.NewGuid(),
                @event.GetType().Name,
                true,
                Serialize(@event),
                Serialize(new DomainEventMetadata { CreationDate = DateTime.UtcNow })
            );
            await _connection.AppendToStreamAsync(GetStreamName(@event.AggregateId), ExpectedVersion.Any, eventData);
            _logger.LogInformation($"{@event.GetType()} has been stored.");
        }

        public async Task<IEnumerable<IDomainEvent>> GetAllEvents(Guid aggregateId) =>
            await ReadAllEventsInStream(GetStreamName(aggregateId), 0)
                .SelectAsync(ConvertToDomainEvent)
                .EnumerateAsync();

        // ----- Internal logic

        private IDomainEvent ConvertToDomainEvent(ResolvedEvent @event)
        {
            if (!_domainEventTypeLocator.TryGetValue(@event.Event.EventType, out var domainEventType)) {
                throw new Exception("Event is unknown, unable to correctly deserialize it.");
            }
            return Deserialize(@event.Event.Data, domainEventType);
        }

        private async Task<IEnumerable<ResolvedEvent>> ReadAllEventsInStream(string streamId, int fromVersion)
        {
            var streamEvents = new List<ResolvedEvent>();
            StreamEventsSlice currentSlice;
            var nextSliceStart = fromVersion == -1 ? StreamPosition.Start : (long) fromVersion;

            do {
                currentSlice = await _connection.ReadStreamEventsForwardAsync(streamId, nextSliceStart, EVENT_COUNT, false);
                if (currentSlice.Status != SliceReadStatus.Success) {
                    throw new InvalidOperationException($"The stream with id {streamId} was not found. Aggregate not found.");
                }
                nextSliceStart = currentSlice.NextEventNumber;
                streamEvents.AddRange(currentSlice.Events);
            } while (!currentSlice.IsEndOfStream);

            return streamEvents;
        }

        private static string GetStreamName(Guid id) => $"transaction-{id}";

        private static byte[] Serialize(object @event)
            => JsonConvert.SerializeObject(@event).Pipe(Encoding.UTF8.GetBytes);

        private static IDomainEvent Deserialize(byte[] eventData, Type domainEventType)
            => eventData
                .Pipe(Encoding.UTF8.GetString)
                .Pipe(x => (IDomainEvent) JsonConvert.DeserializeObject(x, domainEventType));
    }
}