using Microsoft.Extensions.DependencyInjection;
using Sidekick.Core;
using Sidekick.Core.Natives;
using Sidekick.Handlers;
using Sidekick.Natives;
using Sidekick.Views;
using Sidekick.Views.ApplicationLogs;
using Sidekick.Views.Leagues;
using Sidekick.Views.MapInfo;
using Sidekick.Views.Prices;
using Sidekick.Views.Settings;
using Sidekick.Views.SplashScreen;
using Sidekick.Views.TrayIcon;

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
            services.AddScoped<MapInfoView>();
            services.AddScoped<SettingsView>();
            services.AddScoped<SplashScreenView>();

            services.AddScoped<ApplicationLogViewModel>();
            services.AddScoped<LeagueViewModel>();
            services.AddScoped<PriceViewModel>();
            services.AddScoped<MapInfoViewModel>();
            services.AddScoped<SettingsViewModel>();
            services.AddScoped<SplashViewModel>();

            services.AddInitializableService<IKeybindEvents, KeybindEvents>();
            services.AddInitializableService<INativeKeyboard, NativeKeyboard>();
            services.AddInitializableService<INativeProcess, NativeProcess>();
            services.AddInitializableService<INativeCursor, NativeCursor>();
            services.AddSingleton<TrayIconViewModel>();
            services.AddSingleton<EventsHandler>();
            services.AddSingleton<HookProvider>();

            services.AddSingleton<IViewLocator, ViewLocator>();

            return services;
        }
    }
}
