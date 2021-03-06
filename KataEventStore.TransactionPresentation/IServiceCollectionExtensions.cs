using System;
using EventStore.ClientAPI;
using KataEventStore.Events;
using KataEventStore.TransactionPresentation.Projections;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace KataEventStore.TransactionPresentation {
    public static class IServiceCollectionExtensions
    {
        public static IServiceCollection RegisterApplicationServices(this IServiceCollection services)
        {
            services.AddSingleton<InMemoryDatabase>();
            services.AddScoped<IDomainEventTypeLocator, ReflectionDomainEventTypeLocator>();
            services.AddScoped(x => {
                var connection = EventStoreConnection.Create(
                    new Uri("tcp://admin:changeit@localhost:1113")
                );
                connection.ErrorOccurred += (sender, args) => {
                    x.GetRequiredService<ILogger<IEventStoreConnection>>().LogError("The connection was closed.", args.Exception);
                };
                connection.ConnectAsync().Wait();
                return connection;
            });
            services.AddHostedService<ProjectionFeederService>();
            return services;
        }
    }
}