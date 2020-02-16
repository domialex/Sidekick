using Microsoft.Extensions.DependencyInjection;
using Sidekick.Core;
using Sidekick.Core.Natives;
using Sidekick.Helpers.Input;
using Sidekick.Natives;
using Sidekick.Windows;
using Sidekick.Windows.ApplicationLogs;
using Sidekick.Windows.LeagueOverlay;
using Sidekick.Windows.Leagues;
using Sidekick.Windows.Prediction;
using Sidekick.Windows.PriceCheck;
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

            services.AddScoped<LeagueView, LeagueView>();
            services.AddScoped<SettingsView, SettingsView>();
            services.AddScoped<SplashScreen, SplashScreen>();

            services.AddInitializableService<TrayIconViewModel, TrayIconViewModel>();
            services.AddInitializableService<IKeybindEvents, KeybindEvents>();
            services.AddInitializableService<INativeKeyboard, NativeKeyboard>();
            services.AddInitializableService<INativeProcess, NativeProcess>();
            services.AddSingleton<EventsHandler>();
            services.AddSingleton<OverlayController>();
            services.AddSingleton<OverlayWindow>();
            services.AddSingleton<LeagueOverlayController>();
            services.AddSingleton<PredictionController>();
            services.AddSingleton<ApplicationLogsController>();

            return services;
        }
    }
}
