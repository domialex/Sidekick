using Microsoft.Extensions.DependencyInjection;
using Sidekick.Business.Apis.PoeNinja;
using Sidekick.Core;
using Sidekick.Services;
using Sidekick.UI.Settings;
using Sidekick.Windows.Settings;
using Sidekick.Windows.TrayIcon;

namespace Sidekick
{
    public static class StartupExtensions
    {
        public static IServiceCollection AddSidekickServices(this IServiceCollection services)
        {
            services.AddInitializableService<ITrayService, TrayService>();
            services.AddInitializableService<ITrayIconViewModel, TrayIconViewModel>();

            return services;
        }

        public static IServiceCollection AddPoeNinja(this IServiceCollection services)
        {
            services.AddSingleton<IPoeNinjaClient, PoeNinjaClient>();
            services.AddInitializableService<IPoeNinjaCache, PoeNinjaCache>();
            return services;
        }

        public static IServiceCollection AddSidekickUIWindows(this IServiceCollection services)
        {
            services.AddScoped<ISettingView, SettingsView>();
            return services;
        }

    }
}
