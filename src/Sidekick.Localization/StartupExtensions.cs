using Microsoft.Extensions.DependencyInjection;
using Sidekick.Localization.Cheatsheets;
using Sidekick.Localization.Initialization;
using Sidekick.Localization.Platforms;

namespace Sidekick.Localization
{
    public static class StartupExtensions
    {
        public static IServiceCollection AddSidekickLocalization(this IServiceCollection services)
        {
            services.AddTransient<BetrayalResources>();
            services.AddTransient<CheatsheetResources>();
            services.AddTransient<InitializationResources>();
            services.AddTransient<PlatformResources>();

            return services;
        }
    }
}
