using Microsoft.Extensions.DependencyInjection;
using Sidekick.Core;
using Sidekick.Core.Natives;
using Sidekick.Handlers;
using Sidekick.Natives;
using Sidekick.Notifications;
using Sidekick.Views;
using Sidekick.Views.About;
using Sidekick.Views.ApplicationLogs;
using Sidekick.Views.Initialize;
using Sidekick.Views.Leagues;
using Sidekick.Views.MapInfo;
using Sidekick.Views.Prices;
using Sidekick.Views.Settings;
using Sidekick.Views.TrayIcon;

namespace Sidekick
{
    public static class StartupExtensions
    {
        public static IServiceCollection AddSidekickUIWindows(this IServiceCollection services)
        {
            services.AddSingleton<INativeBrowser, NativeBrowser>();
            services.AddSingleton<INativeClipboard, NativeClipboard>();
            services.AddSingleton<INotificationManager, NotificationManager>();

            services.AddScoped<AboutView>();
            services.AddScoped<ApplicationLogsView>();
            services.AddScoped<InitializeView>();
            services.AddScoped<LeagueView>();
            services.AddScoped<PriceView>();
            services.AddScoped<MapInfoView>();
            services.AddScoped<SettingsView>();

            services.AddScoped<ApplicationLogViewModel>();
            services.AddScoped<LeagueViewModel>();
            services.AddScoped<PriceViewModel>();
            services.AddScoped<MapInfoViewModel>();
            services.AddScoped<SettingsViewModel>();
            services.AddScoped<InitializeViewModel>();

            services.AddInitializableService<IKeybindEvents, KeybindEvents>();
            services.AddInitializableService<INativeKeyboard, NativeKeyboard>();
            services.AddInitializableService<INativeProcess, NativeProcess>();
            services.AddInitializableService<INativeCursor, NativeCursor>();
            services.AddInitializableService<EventsHandler, EventsHandler>();
            services.AddSingleton<TrayIconViewModel>();
            services.AddSingleton<HookProvider>();

            services.AddSingleton<IViewLocator, ViewLocator>();

            return services;
        }
    }
}
