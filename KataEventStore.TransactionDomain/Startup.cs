using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using Serilog.Extensions.Logging;
using Swashbuckle.AspNetCore.SwaggerUI;

namespace KataEventStore.TransactionDomain
{
    public class Startup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddSingleton<ILoggerFactory>(services => new SerilogLoggerFactory());

            services
                .AddLogging()
                .AddMediatR(typeof(Startup).Assembly)
                .AddSwaggerGen(conf => conf.SwaggerDoc("v1.0", new OpenApiInfo { Title = "TransactionDomain API v1.0", Version = "v1.0" }))
                .AddControllers();

            services.RegisterApplicationServices();
        }

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
}