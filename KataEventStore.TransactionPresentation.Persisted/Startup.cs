using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerUI;

namespace KataEventStore.TransactionPresentation.Persisted
{
    public class Startup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            services
                .AddMediatR(typeof(Startup).Assembly)
                .AddSwaggerGen(conf => conf.SwaggerDoc("v1.0", new OpenApiInfo { Title = "TransactionPresentation.Persisted API v1.0", Version = "v1.0" }))
                .AddControllers();

            services.RegisterApplicationServices();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app
                .UseRouting()
                .UseEndpoints(endpoints => endpoints.MapControllers())
                .UseSwagger()
                .UseSwaggerUI(c => {
                    c.SwaggerEndpoint("/swagger/v1.0/swagger.json", "TransactionPresentation.Persisted API v1.0");
                    c.DocExpansion(DocExpansion.None);
                    c.RoutePrefix = string.Empty;
                });
        }
    }
}
