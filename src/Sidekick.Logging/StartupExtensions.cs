using System;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;
using Sidekick.Extensions;

namespace Sidekick.Logging
{

    public static class StartupExtensions
    {
        public static IServiceCollection AddSidekickLogging(this IServiceCollection services, IConfiguration configuration, IHostEnvironment environment)
        {
            var sidekickPath = Environment.ExpandEnvironmentVariables("%AppData%\\sidekick");

            var logSink = new LogSink();

            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Debug()
                .MinimumLevel.Override("Microsoft", Serilog.Events.LogEventLevel.Information)
                .Enrich.FromLogContext()
                .WriteTo.File(SidekickPaths.GetDataFilePath("Sidekick_log.log"),
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
