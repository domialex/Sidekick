using Microsoft.Extensions.DependencyInjection;
using Sidekick.Core.Natives;
using Sidekick.Handlers;
using Sidekick.Initialization;
using Sidekick.Natives;
using Sidekick.Presentation.Views;
using Sidekick.Setup;
using Sidekick.Views;
using Sidekick.Views.About;
using Sidekick.Views.ApplicationLogs;
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
            services.AddSingleton<INativeNotifications, NativeNotifications>();

            services.AddScoped<AboutView>();
            services.AddScoped<ApplicationLogsView>();
            services.AddScoped<InitializationView>();
            services.AddScoped<LeagueView>();
            services.AddScoped<PriceView>();
            services.AddScoped<MapInfoView>();
            services.AddScoped<SettingsView>();
            services.AddScoped<SetupView>();

            services.AddScoped<ApplicationLogViewModel>();
            services.AddScoped<LeagueViewModel>();
            services.AddScoped<PriceViewModel>();
            services.AddScoped<MapInfoViewModel>();
            services.AddScoped<SettingsViewModel>();
            services.AddSingleton<InitializationViewModel>();
            services.AddScoped<SetupViewModel>();

            services.AddSingleton<IKeybindEvents, KeybindEvents>();
            services.AddSingleton<INativeKeyboard, NativeKeyboard>();
            services.AddSingleton<INativeProcess, NativeProcess>();
            services.AddSingleton<INativeCursor, NativeCursor>();
            services.AddSingleton<EventsHandler, EventsHandler>();
            services.AddSingleton<TrayIconViewModel>();
            services.AddSingleton<HookProvider>();

            services.AddSingleton<IViewLocator, ViewLocator>();

            return services;
        }
    }
}
