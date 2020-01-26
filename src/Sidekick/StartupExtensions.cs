using Microsoft.Extensions.DependencyInjection;
using Sidekick.Core;
using Sidekick.Services;
using Sidekick.Windows.Settings;
using Sidekick.Windows.TrayIcon;

namespace Sidekick
{
    public static class StartupExtensions
    {
        public static IServiceCollection AddSidekickUIWindows(this IServiceCollection services)
        {
            services.AddScoped<SettingsView, SettingsView>();

            services.AddInitializableService<ITrayService, TrayService>();
            services.AddInitializableService<ITrayIconViewModel, TrayIconViewModel>();

            return services;
        }

    }
}
