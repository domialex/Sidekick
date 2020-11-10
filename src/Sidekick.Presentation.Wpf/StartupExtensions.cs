using Microsoft.Extensions.DependencyInjection;
using Sidekick.Debounce;
using Sidekick.Domain.Keybinds;
using Sidekick.Domain.Process;
using Sidekick.Domain.Views;
using Sidekick.Errors;
using Sidekick.Initialization;
using Sidekick.Keybinds;
using Sidekick.Natives;
using Sidekick.Setup;
using Sidekick.Views;
using Sidekick.Views.About;
using Sidekick.Views.ApplicationLogs;
using Sidekick.Views.Leagues;
using Sidekick.Views.MapInfo;
using Sidekick.Views.Prices;
using Sidekick.Views.Settings;
using Sidekick.Views.TrayIcon;

namespace Sidekick.Presentation.Wpf
{
    public static class StartupExtensions
    {
        public static IServiceCollection AddSidekickUIWindows(this IServiceCollection services)
        {
            services.AddSingleton<IDebouncer, Debouncer>();

            services.AddScoped<AboutView>();
            services.AddScoped<ApplicationLogsView>();
            services.AddScoped<InitializationView>();
            services.AddScoped<LeagueView>();
            services.AddScoped<PriceView>();
            services.AddScoped<MapInfoView>();
            services.AddScoped<SettingsView>();
            services.AddScoped<SetupView>();
            services.AddScoped<ParserError>();

            services.AddScoped<ApplicationLogViewModel>();
            services.AddScoped<LeagueViewModel>();
            services.AddScoped<PriceViewModel>();
            services.AddScoped<MapInfoViewModel>();
            services.AddScoped<SettingsViewModel>();
            services.AddSingleton<InitializationViewModel>();
            services.AddScoped<SetupViewModel>();

            services.AddSingleton<INativeProcess, NativeProcess>();
            services.AddSingleton<TrayIconViewModel>();
            services.AddSingleton<IKeybindsProvider, KeybindsProvider>();

            services.AddSingleton<IViewLocator, ViewLocator>();

            return services;
        }
    }
}
