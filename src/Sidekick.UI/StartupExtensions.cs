using Microsoft.Extensions.DependencyInjection;
using Sidekick.UI.ApplicationLogs;
using Sidekick.UI.Leagues;
using Sidekick.UI.Prices;
using Sidekick.UI.Settings;
using Sidekick.UI.Splash;
using Sidekick.UI.Views;

namespace Sidekick.UI
{
    public static class StartupExtensions
    {
        public static IServiceCollection AddSidekickUIServices(this IServiceCollection services)
        {
            services.AddSingleton<IViewLocator, ViewLocator>();

            services.AddScoped<IApplicationLogViewModel, ApplicationLogViewModel>();
            services.AddScoped<ILeagueViewModel, LeagueViewModel>();
            services.AddScoped<IPriceViewModel, PriceViewModel>();
            services.AddScoped<ISettingsViewModel, SettingsViewModel>();
            services.AddScoped<ISplashViewModel, SplashViewModel>();

            return services;
        }
    }
}
