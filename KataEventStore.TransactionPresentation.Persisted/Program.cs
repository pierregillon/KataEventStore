using FluentAsync;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Serilog;

namespace KataEventStore.TransactionPresentation.Persisted
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder
                    => webBuilder.UseStartup<Startup>().ConfigureLogging(SerilogConfiguration)
                );

        private static void SerilogConfiguration(WebHostBuilderContext builderContext, ILoggingBuilder builder)
        {
            builder.ClearProviders();

            new LoggerConfiguration()
                .ReadFrom
                .Configuration(builderContext.Configuration)
                .CreateLogger()
                .Pipe(x => builder.AddSerilog(x));
        }
    }
}