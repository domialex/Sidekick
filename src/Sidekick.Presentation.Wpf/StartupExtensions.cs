using Microsoft.Extensions.DependencyInjection;
using Sidekick.Domain.Keybinds;
using Sidekick.Domain.Process;
using Sidekick.Domain.Views;
using Sidekick.Presentation.Wpf.About;
using Sidekick.Presentation.Wpf.Cheatsheets;
using Sidekick.Presentation.Wpf.Debounce;
using Sidekick.Presentation.Wpf.Errors;
using Sidekick.Presentation.Wpf.Initialization;
using Sidekick.Presentation.Wpf.Keybinds;
using Sidekick.Presentation.Wpf.Natives;
using Sidekick.Presentation.Wpf.Settings;
using Sidekick.Presentation.Wpf.Setup;
using Sidekick.Presentation.Wpf.Views;
using Sidekick.Presentation.Wpf.Views.ApplicationLogs;
using Sidekick.Presentation.Wpf.Views.MapInfo;
using Sidekick.Presentation.Wpf.Views.Prices;
using Sidekick.Presentation.Wpf.Views.TrayIcon;

namespace Sidekick.Presentation.Wpf
{
    public static class StartupExtensions
    {
        public static IServiceCollection AddSidekickPresentationWpf(this IServiceCollection services)
        {
            services.AddSingleton<IDebouncer, Debouncer>();

            services.AddScoped<AboutView>();

            services.AddScoped<ApplicationLogsView>();
            services.AddScoped<ApplicationLogViewModel>();

            services.AddScoped<AvailableInEnglishError>();

            services.AddScoped<InitializationView>();
            services.AddSingleton<InitializationViewModel>();

            services.AddScoped<InvalidItemError>();

            services.AddScoped<LeagueView>();
            services.AddScoped<LeagueViewModel>();

            services.AddScoped<ParserError>();

            services.AddScoped<PriceView>();
            services.AddScoped<PriceViewModel>();

            services.AddScoped<MapInfoView>();
            services.AddScoped<MapInfoViewModel>();

            services.AddScoped<SettingsView>();
            services.AddScoped<SettingsViewModel>();

            services.AddScoped<SetupView>();
            services.AddScoped<SetupViewModel>();

            services.AddSingleton<INativeProcess, NativeProcess>();
            services.AddSingleton<TrayIconViewModel>();
            services.AddSingleton<IKeybindsProvider, KeybindsProvider>();

            services.AddSingleton<IViewLocator, ViewLocator>();

            return services;
        }
    }
}
