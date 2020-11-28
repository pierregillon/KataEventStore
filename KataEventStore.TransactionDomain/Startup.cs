using EventStore.ClientAPI;
using KataEventStore.TransactionDomain.Domain.Core._Base;
using KataEventStore.TransactionDomain.Domain.Infrastructure;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerUI;

namespace KataEventStore.TransactionDomain
{
    public class Startup
    {
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services
                .AddMediatR(typeof(Startup).Assembly)
                .AddSwaggerGen(conf => conf.SwaggerDoc("v1.0", new OpenApiInfo { Title = "TransactionDomain API v1.0", Version = "v1.0" }))
                .AddControllers();

            services.RegisterApplicationServices();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment()) {
                app.UseDeveloperExceptionPage();
            }

            app
                .UseRouting()
                .UseEndpoints(endpoints => endpoints.MapControllers())
                .UseSwagger()
                .UseSwaggerUI(c => {
                    c.SwaggerEndpoint("/swagger/v1.0/swagger.json", "TransactionDomain API v1.0");
                    c.DocExpansion(DocExpansion.None);
                    c.RoutePrefix = string.Empty;
                });
        }
    }

    public static class IServiceCollectionExtensions
    {
        public static IServiceCollection RegisterApplicationServices(this IServiceCollection services)
        {
            services.AddScoped<IEventStore, EventStoreOrg>();
            //services.AddScoped(x => EventStoreConnection.Create("http://127.0.0.7:1113"));

            return services;
        }
    }
}