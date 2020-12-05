using System;
using EventStore.ClientAPI;
using EventStore.ClientAPI.SystemData;
using KataEventStore.Events;
using KataEventStore.TransactionPresentation.Projections;
using Microsoft.Extensions.DependencyInjection;

namespace KataEventStore.TransactionPresentation {
    public static class IServiceCollectionExtensions
    {
        public static IServiceCollection RegisterApplicationServices(this IServiceCollection services)
        {
            services.AddSingleton<InMemoryDatabase>();
            services.AddScoped<IDomainEventTypeLocator, ReflectionDomainEventTypeLocator>();
            services.AddScoped<IEventStoreConnection>(x => {
                var settings = ConnectionSettings
                    .Create()
                    .SetDefaultUserCredentials(new UserCredentials("admin", "changeit"))
                    .UseConsoleLogger()
                    .KeepReconnecting()
                    .Build();

                //new Uri("tcp://admin:changeit@localhost:1113")

                var connection = EventStoreConnection.Create(
                    settings, new Uri("tcp://admin:changeit@localhost:1113")
                );
                connection.ConnectAsync().Wait();
                return connection;
            });
            services.AddHostedService<ProjectionFeederService>();
            return services;
        }
    }
}