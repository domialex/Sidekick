using Microsoft.Extensions.DependencyInjection;
using Sidekick.UI.Settings;
using Sidekick.UI.Views;

namespace Sidekick.UI
{
    public static class StartupExtensions
    {
        public static IServiceCollection AddSidekickUIServices(this IServiceCollection services)
        {
            services.AddSingleton<IViewController, ViewController>();

            services.AddScoped<ISettingsViewModel, SettingsViewModel>();

            return services;
        }
    }
}
