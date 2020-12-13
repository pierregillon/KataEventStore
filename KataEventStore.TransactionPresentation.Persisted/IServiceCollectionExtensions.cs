using System;
using EventStore.ClientAPI;
using EventStore.ClientAPI.SystemData;
using KataEventStore.Events;
using KataEventStore.TransactionPresentation.Persisted.Projections;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace KataEventStore.TransactionPresentation.Persisted {
    public static class IServiceCollectionExtensions
    {
        public static IServiceCollection RegisterApplicationServices(this IServiceCollection services)
        {
            services.AddSingleton<ITransactionRepository, FileTransactionRepository>();
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