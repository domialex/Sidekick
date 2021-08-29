using System;
using Microsoft.Extensions.DependencyInjection;
using Serilog;
using Sidekick.Common.Browser;
using Sidekick.Common.Cache;
using Sidekick.Common.Localization;
using Sidekick.Common.Logging;

namespace Sidekick.Common
{
    public static class StartupExtensions
    {
        public static IServiceCollection AddSidekickCommon(this IServiceCollection services)
        {
            services.AddSingleton<ICacheProvider, CacheProvider>();

            // Logging
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

            services.AddSingleton<IUILanguageProvider, UILanguageProvider>();

            services.AddSingleton<IBrowserProvider, BrowserProvider>();

            return services;
        }
    }
}
