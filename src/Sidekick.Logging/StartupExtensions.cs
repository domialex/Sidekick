using System.IO;
using System.Reflection;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Sentry;
using Serilog;
using Sidekick.Domain.Settings;

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
                .WriteTo.File(Path.Combine(Path.GetDirectoryName(Assembly.GetEntryAssembly().Location), "Sidekick_log.log"),
                    rollingInterval: RollingInterval.Day,
                    retainedFileCountLimit: 1,
                    fileSizeLimitBytes: 5242880,
                    rollOnFileSizeLimit: true)
                .WriteTo.Sink(logSink)
                .AddSentryLogging(services)
                .CreateLogger();

            services.AddLogging(builder =>
            {
                builder.AddSerilog();
            });

            services.AddSingleton(logSink);

            return services;
        }

        public static LoggerConfiguration AddSentryLogging(this LoggerConfiguration loggerConfiguration, IServiceCollection services)
        {
            var serviceProvider = services.BuildServiceProvider();
            var sidekickSettings = serviceProvider.GetService<ISidekickSettings>();
            var environment = serviceProvider.GetService<IHostingEnvironment>();

            loggerConfiguration
                .WriteTo
                .Conditional(x => sidekickSettings.SendCrashReports,
                             c => c.Sentry(o =>
                             {
                                 o.Dsn = "https://7182a08eae7443a8a1b6aae8e64a0adb@o152592.ingest.sentry.io/5645809";
                                 o.Environment = environment.EnvironmentName;
                                 o.BeforeSend += e =>
                                 {
                                     e.User = new User() { Id = sidekickSettings.UserId.ToString(), Username = sidekickSettings.Character_Name };
                                     return e;
                                 };
                             }));

            return loggerConfiguration;
        }

    }
}
