using System.IO;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
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
    }
}
