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
            services.AddSingleton<ILogger, Logger>();
            services.AddSingleton<IInitializer, Initializer>();

            return services;
        }
    }
}
