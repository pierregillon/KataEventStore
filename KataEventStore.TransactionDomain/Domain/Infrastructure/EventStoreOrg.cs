﻿using System;
using System.Collections.Generic;
using System.Linq;
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

        private readonly JsonSerializerSettings _serializerSettings;
        private readonly IDomainEventTypeLocator _domainEventTypeLocator;
        private readonly IEventStoreConnection _connection;
        private readonly ILogger<EventStoreOrg> _logger;

        public EventStoreOrg(IDomainEventTypeLocator domainEventTypeLocator, IEventStoreConnection connection, ILogger<EventStoreOrg> logger)
        {
            _domainEventTypeLocator = domainEventTypeLocator;
            _connection = connection;
            _logger = logger;
            _serializerSettings = new JsonSerializerSettings {
                Formatting = Formatting.Indented
            };
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
                Serialize(new DomainEventMetadata{ CreationDate = DateTime.UtcNow })
            );
            await _connection.AppendToStreamAsync(GetStreamName(@event.AggregateId), ExpectedVersion.Any, eventData);
            _logger.LogInformation($"{@event.GetType()} has been stored.");
        }

        private byte[] Serialize(object @event) => JsonConvert.SerializeObject(@event, _serializerSettings).Pipe(Encoding.UTF8.GetBytes);

        public async Task<IEnumerable<IDomainEvent>> GetAllEvents(Guid aggregateId) =>
            await ReadAllEventsInStream(GetStreamName(aggregateId), 0)
                .SelectAsync(ConvertToDomainEvent)
                .EnumerateAsync();

        public async IAsyncEnumerable<IDomainEvent> GetAllEventsBetween(Position startPosition, Position endPosition, IReadOnlyCollection<Type> eventTypes)
        {
            var eventTypesByName = eventTypes.ToDictionary(x => x.Name);

            AllEventsSlice currentSlice;

            do {
                currentSlice = await _connection.ReadAllEventsForwardAsync(startPosition, EVENT_COUNT, false);
                startPosition = currentSlice.NextPosition;
                foreach (var @event in currentSlice.Events.Where(x => !x.Event.EventType.StartsWith("$"))) {
                    if (eventTypesByName.TryGetValue(@event.Event.EventType, out var eventType)) {
                        yield return ConvertToDomainEvent(@event, eventType);
                    }
                    if (@event.OriginalPosition == endPosition) {
                        yield break;
                    }
                }
            } while (!currentSlice.IsEndOfStream);
        }

        public async Task<long> GetCurrentGlobalStreamPosition()
        {
            var slice = await _connection.ReadAllEventsBackwardAsync(Position.End, 1, false);
            return slice.FromPosition.CommitPosition;
        }

        // ----- Internal logic

        private IDomainEvent ConvertToDomainEvent(ResolvedEvent @event)
        {
            if (_domainEventTypeLocator.TryGetValue(@event.Event.EventType, out var type)) {
                return ConvertToDomainEvent(@event, type);
            }
            throw new Exception("Event is unknown, unable to correctly deserialize it.");
        }

        private IDomainEvent ConvertToDomainEvent(ResolvedEvent @event, Type eventType)
        {
            var json = Encoding.UTF8.GetString(@event.Event.Data);
            var domainEvent = (IDomainEvent) JsonConvert.DeserializeObject(json, eventType, _serializerSettings);
            return domainEvent;
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
        public static string GetStreamName(Guid id) => $"transaction-{id}";
    }

}