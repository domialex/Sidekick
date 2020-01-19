using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Sidekick.Core.Initialization;
using Sidekick.Core.Loggers;
using System.IO;

namespace Sidekick.Core
{
    public static class StartupExtensions
    {
        public static IServiceCollection AddSidekickConfiguration(this IServiceCollection services)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile(Configuration.Configuration.FileName, optional: true, reloadOnChange: true);

            IConfigurationRoot configuration = builder.Build();

            services.AddSingleton(typeof(IConfigurationRoot), configuration);
            services.AddSingleton(typeof(IConfiguration), configuration);

            var sidekickConfiguration = new Configuration.Configuration();
            configuration.Bind(sidekickConfiguration);
            services.AddSingleton(sidekickConfiguration);

            return services;
        }

        public static IServiceCollection AddSidekickCoreServices(this IServiceCollection services)
        {
            services.AddSingleton<IInitializer, Initializer>();
            services.AddInitializableService<ILogger, Logger>();

            return services;
        }

        private static readonly HashSet<Type> intializeTypes = new HashSet<Type>
        {
            typeof(IOnReset),
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
