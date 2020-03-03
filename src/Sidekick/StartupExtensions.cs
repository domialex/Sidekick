using Microsoft.Extensions.DependencyInjection;
using Sidekick.Core;
using Sidekick.Core.Natives;
using Sidekick.Handlers;
using Sidekick.Natives;
using Sidekick.Windows;
using Sidekick.Windows.AdvancedSearch;
using Sidekick.Windows.ApplicationLogs;
using Sidekick.Windows.Leagues;
using Sidekick.Windows.Prices;
using Sidekick.Windows.Settings;
using Sidekick.Windows.TrayIcon;

namespace Sidekick
{
    public static class StartupExtensions
    {
        public static IServiceCollection AddSidekickUIWindows(this IServiceCollection services)
        {
            services.AddSingleton<INativeBrowser, NativeBrowser>();
            services.AddSingleton<INativeClipboard, NativeClipboard>();

            services.AddScoped<ApplicationLogsView>();
            services.AddScoped<LeagueView>();
            services.AddScoped<PriceView>();
            services.AddScoped<SettingsView>();
            services.AddScoped<SplashScreen>();

            services.AddInitializableService<IKeybindEvents, KeybindEvents>();
            services.AddInitializableService<INativeKeyboard, NativeKeyboard>();
            services.AddInitializableService<INativeProcess, NativeProcess>();
            services.AddInitializableService<INativeCursor, NativeCursor>();
            services.AddSingleton<TrayIconViewModel>();
            services.AddSingleton<EventsHandler>();
            services.AddSingleton<AdvancedSearchController>();

            return services;
        }
    }
}
