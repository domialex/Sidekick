using Microsoft.Extensions.DependencyInjection;
using Sidekick.Core;
using Sidekick.Core.Natives;
using Sidekick.Natives;
using Sidekick.Services;
using Sidekick.Windows;
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

            services.AddScoped<SettingsView, SettingsView>();
            services.AddScoped<SplashScreen, SplashScreen>();

            services.AddInitializableService<ITrayService, TrayService>();
            services.AddInitializableService<ITrayIconViewModel, TrayIconViewModel>();
            services.AddInitializableService<IKeybindEvents, KeybindEvents>();
            services.AddInitializableService<INativeKeyboard, NativeKeyboard>();
            services.AddInitializableService<INativeProcess, NativeProcess>();

            return services;
        }
    }
}
