using System;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using EventStore.ClientAPI;
using EventStore.ClientAPI.SystemData;
using FluentAsync;
using KataEventStore.Events;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace KataEventStore.TransactionPresentation.Persisted.Projections
{
    public class ProjectionFeederService : IHostedService
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly ILogger<ProjectionFeederService> _logger;
        private IServiceScope _scope;
        private EventStorePersistentSubscriptionBase _persistentSubscription;

        public ProjectionFeederService(IServiceProvider serviceProvider, ILogger<ProjectionFeederService> logger)
        {
            _serviceProvider = serviceProvider;
            _logger = logger;
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            _scope = _serviceProvider.CreateScope();

            await SubscribeToEvents(
                _scope.ServiceProvider.GetRequiredService<IMediator>(),
                _scope.ServiceProvider.GetRequiredService<IEventStoreConnection>(),
                _scope.ServiceProvider.GetRequiredService<IDomainEventTypeLocator>(),
                cancellationToken
            );
        }

        private async Task SubscribeToEvents(IPublisher mediator, IEventStoreConnection eventStoreConnection, IDomainEventTypeLocator domainEventTypeLocator, CancellationToken cancellationToken)
        {
            async Task Publish(RecordedEvent @event, Type domainEventType)
            {
                var domainEvent = (IDomainEvent)Deserialize(@event.Data, domainEventType);
                _logger.LogInformation($"Playing event {domainEvent.GetType().Name} of aggregate {domainEvent.AggregateId}");
                await mediator.Publish(domainEvent, cancellationToken);
            }

            await CreatePersistentSubscriptionIfDoesNotExists(eventStoreConnection);

            _persistentSubscription = await eventStoreConnection.ConnectToPersistentSubscriptionAsync(
                "$ce-transaction",
                "TransactionPresentation",
                async (subscription, @event) => {
                    if (domainEventTypeLocator.TryGetValue(@event.Event.EventType, out var domainEventType)) {
                        await Publish(@event.Event, domainEventType);
                    }
                    subscription.Acknowledge(@event);
                },
                (subscription, reason, error) => {
                    _logger.LogError(error, $"The subscription dropped because of {reason}");
                }
            );
        }

        private async Task CreatePersistentSubscriptionIfDoesNotExists(IEventStoreConnection eventStoreConnection)
        {
            try {
                var settings = PersistentSubscriptionSettings
                    .Create()
                    .ResolveLinkTos()
                    .StartFromBeginning();

                await eventStoreConnection.CreatePersistentSubscriptionAsync(
                    "$ce-transaction",
                    "TransactionPresentation",
                    settings,
                    new UserCredentials("admin", "changeit")
                );
            }
            catch (Exception ex) {
                _logger.LogWarning(ex, "Persistent subscription exists");
            }
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            _persistentSubscription?.Stop(TimeSpan.FromSeconds(10));
            _scope.Dispose();
            return Task.CompletedTask;
        }

        private static object Deserialize(byte[] data, Type type)
        {
            return data
                .Pipe(Encoding.UTF8.GetString)
                .Pipe(x => JsonConvert.DeserializeObject(x, type));
        }
    }
}