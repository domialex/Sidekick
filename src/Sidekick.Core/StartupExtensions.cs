using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Serilog;
using Sidekick.Core.Initialization;
using Sidekick.Core.Logging;
using Sidekick.Core.Settings;
using Sidekick.Core.Update;

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

            services.AddSingleton<IInitializer, Initializer>();
            services.AddSingleton<IUpdateManager, UpdateManager>();
            return services;
        }

        private static readonly HashSet<Type> intializeTypes = new HashSet<Type>
        {
            typeof(IOnBeforeInit),
            typeof(IOnInit),
            typeof(IOnAfterInit)
        };


        /// <summary>
        /// Registers a singleton service, along with any intializatation interfaces the implementation type also implements.
        /// Required for integration with <see cref="IInitializer"/>.
        /// </summary>
        /// <typeparam name="TInterface"></typeparam>
        /// <typeparam name="TImplementation"></typeparam>
        /// <param name="services"></param>
        public static void AddInitializableService<TInterface, TImplementation>(this IServiceCollection services)
            where TInterface : class
            where TImplementation : class, TInterface
        {
            services.AddSingleton<TInterface, TImplementation>();

            foreach (var initializeInterface in typeof(TImplementation).GetInterfaces().Where(i => intializeTypes.Contains(i)))
            {
                services.AddSingleton(initializeInterface, serviceProvider => serviceProvider.GetRequiredService<TInterface>());
            }
        }
    }
}
