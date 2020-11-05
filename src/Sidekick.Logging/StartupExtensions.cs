using Microsoft.Extensions.DependencyInjection;
using Serilog;

namespace Sidekick.Logging
{
    public static class StartupExtensions
    {
        public static IServiceCollection AddSidekickLogging(this IServiceCollection services)
        {
            var logSink = new LogSink();

            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Debug()
                .MinimumLevel.Override("Microsoft", Serilog.Events.LogEventLevel.Information)
                .Enrich.FromLogContext()
                .WriteTo.File("Sidekick_Log.log",
                    rollingInterval: RollingInterval.Day,
                    retainedFileCountLimit: 1,
                    fileSizeLimitBytes: 5242880,
                    rollOnFileSizeLimit: true)
                .WriteTo.Sink(logSink)
                .CreateLogger();

            services.AddLogging(builder =>
            {
                builder.AddSerilog();
            });

            services.AddSingleton(logSink);

            return services;
        }
    }
}
