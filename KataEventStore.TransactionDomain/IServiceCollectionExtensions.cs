using System;
using EventStore.ClientAPI;
using KataEventStore.Events;
using KataEventStore.TransactionDomain.Domain.Core._Base;
using KataEventStore.TransactionDomain.Domain.Infrastructure;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace KataEventStore.TransactionDomain
{
    public static class IServiceCollectionExtensions
    {
        public static IServiceCollection RegisterApplicationServices(this IServiceCollection services)
        {
            services.AddScoped<IDomainEventTypeLocator, ReflectionDomainEventTypeLocator>();
            services.AddScoped<IEventStore, EventStoreOrg>();
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

            return services;
        }
    }
}