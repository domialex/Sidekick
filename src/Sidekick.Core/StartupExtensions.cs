using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Sidekick.Core.Initialization;
using Sidekick.Core.Loggers;
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

            services.AddSingleton(typeof(IConfigurationRoot), configuration);
            services.AddSingleton(typeof(IConfiguration), configuration);

            var sidekickConfiguration = DefaultSettings.CreateDefault();
            configuration.Bind(sidekickConfiguration);
            services.AddSingleton(sidekickConfiguration);

            return services;
        }

        public static IServiceCollection AddSidekickCoreServices(this IServiceCollection services)
        {
            services.AddSingleton<IInitializer, Initializer>();
            services.AddInitializableService<ILogger, Logger>();
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
