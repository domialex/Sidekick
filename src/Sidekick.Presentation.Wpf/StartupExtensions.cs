using Microsoft.Extensions.DependencyInjection;
using Sidekick.Domain.Keybinds;
using Sidekick.Domain.Process;
using Sidekick.Domain.Views;
using Sidekick.Presentation.Wpf.Cheatsheets;
using Sidekick.Presentation.Wpf.Debounce;
using Sidekick.Presentation.Wpf.Errors;
using Sidekick.Presentation.Wpf.Initialization;
using Sidekick.Presentation.Wpf.Keybinds;
using Sidekick.Presentation.Wpf.Natives;
using Sidekick.Presentation.Wpf.Setup;
using Sidekick.Presentation.Wpf.Views;
using Sidekick.Presentation.Wpf.Views.About;
using Sidekick.Presentation.Wpf.Views.ApplicationLogs;
using Sidekick.Presentation.Wpf.Views.MapInfo;
using Sidekick.Presentation.Wpf.Views.Prices;
using Sidekick.Presentation.Wpf.Views.Settings;
using Sidekick.Presentation.Wpf.Views.TrayIcon;

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
