using System.IO;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Sidekick.Application.Settings;
using Sidekick.Domain.Settings;

namespace Sidekick.Application
{
    public static class StartupExtensions
    {
        public static IServiceCollection AddSidekickApplication(this IServiceCollection services)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile(SaveSettingsHandler.FileName, optional: true, reloadOnChange: true);

            var configuration = builder.Build();

            var sidekickConfiguration = new SidekickSettings();
            configuration.Bind(sidekickConfiguration);
            services.AddSingleton(sidekickConfiguration);
            services.AddSingleton<ISidekickSettings>(sp => sp.GetRequiredService<SidekickSettings>());

            return services;
        }
    }
}
