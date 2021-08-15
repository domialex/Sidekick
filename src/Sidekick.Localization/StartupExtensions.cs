using Microsoft.Extensions.DependencyInjection;
using Sidekick.Localization.Errors;
using Sidekick.Localization.Initialization;
using Sidekick.Localization.Tray;

namespace Sidekick.Localization
{
    public static class StartupExtensions
    {
        public static IServiceCollection AddSidekickLocalization(this IServiceCollection services)
        {
            services.AddLocalization();

            services.AddTransient<ErrorResources>();

            services.AddTransient<InitializationResources>();

            services.AddTransient<TrayResources>();

            return services;
        }
    }
}
