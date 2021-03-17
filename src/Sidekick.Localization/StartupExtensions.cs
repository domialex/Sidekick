using Microsoft.Extensions.DependencyInjection;
using Sidekick.Localization.About;
using Sidekick.Localization.Cheatsheets;
using Sidekick.Localization.Errors;
using Sidekick.Localization.Initialization;
using Sidekick.Localization.Maps;
using Sidekick.Localization.Platforms;
using Sidekick.Localization.Prices;
using Sidekick.Localization.Settings;
using Sidekick.Localization.Setup;
using Sidekick.Localization.Tray;

namespace Sidekick.Localization
{
    public static class StartupExtensions
    {
        public static IServiceCollection AddSidekickLocalization(this IServiceCollection services)
        {
            services.AddLocalization();

            services.AddTransient<AboutResources>();

            services.AddTransient<BetrayalResources>();
            services.AddTransient<BlightResources>();
            services.AddTransient<CheatsheetResources>();
            services.AddTransient<DelveResources>();
            services.AddTransient<HeistResources>();
            services.AddTransient<IncursionResources>();
            services.AddTransient<MetamorphResources>();

            services.AddTransient<ErrorResources>();

            services.AddTransient<InitializationResources>();

            services.AddTransient<PlatformResources>();

            services.AddTransient<PriceResources>();

            services.AddTransient<SettingsResources>();

            services.AddTransient<MapInfoResources>();

            services.AddTransient<SetupResources>();

            services.AddTransient<TrayResources>();

            return services;
        }
    }
}
