using System.IO;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Serilog;
using Sidekick.Core.Debounce;
using Sidekick.Core.Initialization;
using Sidekick.Core.Logging;
using Sidekick.Core.Settings;

namespace Sidekick.Core
{
    public static class StartupExtensions
    {
        public static IServiceCollection AddSidekickConfiguration(this IServiceCollection services)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile(SidekickSettings.FileName, optional: true, reloadOnChange: true);

            var configuration = builder.Build();

            var sidekickConfiguration = DefaultSettings.CreateDefault();
            configuration.Bind(sidekickConfiguration);
            services.AddSingleton(sidekickConfiguration);

            return services;
        }

        public static IServiceCollection AddSidekickCoreServices(this IServiceCollection services)
        {
            var eventSink = new SidekickEventSink();

            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Debug()
                .MinimumLevel.Override("Microsoft", Serilog.Events.LogEventLevel.Information)
                .Enrich.FromLogContext()
                .WriteTo.File("Sidekick_Log.log",
                    rollingInterval: RollingInterval.Day,
                    retainedFileCountLimit: 1,
                    fileSizeLimitBytes: 5242880,
                    rollOnFileSizeLimit: true)
                .WriteTo.Sink(eventSink)
                .CreateLogger();

            services.AddSingleton(eventSink);
            services.AddSingleton(Log.Logger);

            services.AddSingleton<IDebouncer, Debouncer>();
            services.AddSingleton<IInitializer, Initializer>();

            return services;
        }
    }
}
