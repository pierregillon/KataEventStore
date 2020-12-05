using System;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using EventStore.ClientAPI;
using KataEventStore.Events;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace KataEventStore.TransactionPresentation.Projections
{
    public class ProjectionFeederService : IHostedService
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly ILogger<ProjectionFeederService> _logger;
        private EventStoreAllCatchUpSubscription _subscription;
        private IServiceScope _scope;

        public ProjectionFeederService(IServiceProvider serviceProvider, ILogger<ProjectionFeederService> logger)
        {
            _serviceProvider = serviceProvider;
            _logger = logger;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            _scope = _serviceProvider.CreateScope();

            SubscribeToAllEvents(
                _scope.ServiceProvider.GetRequiredService<IMediator>(),
                _scope.ServiceProvider.GetRequiredService<IEventStoreConnection>(),
                _scope.ServiceProvider.GetRequiredService<IDomainEventTypeLocator>(),
                cancellationToken
            );

            return Task.CompletedTask;
        }

        private void SubscribeToAllEvents(IPublisher mediator, IEventStoreConnection eventStoreConnection, IDomainEventTypeLocator domainEventTypeLocator, CancellationToken cancellationToken)
        {
            _subscription = eventStoreConnection.SubscribeToAllFrom(
                Position.Start,
                CatchUpSubscriptionSettings.Default,
                async (subscription, @event) => {
                    if (domainEventTypeLocator.TryGetValue(@event.Event.EventType, out var domainEventType)) {
                        var json = Encoding.UTF8.GetString(@event.Event.Data);
                        var domainEvent = (IDomainEvent) JsonConvert.DeserializeObject(json, domainEventType);
                        _logger.LogInformation($"Replaying event {domainEvent.GetType().Name} on aggregate {domainEvent.AggregateId}");
                        await mediator.Publish(domainEvent, cancellationToken);
                    }
                },
                x => _logger.LogInformation("Moving to live mode")
            );
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            _subscription?.Stop();
            _scope.Dispose();
            return Task.CompletedTask;
        }
    }
}