using Microsoft.Extensions.DependencyInjection;
using Sidekick.UI.Settings;

namespace Sidekick.UI
{
    public static class StartupExtensions
    {
        public static IServiceCollection AddSidekickUIServices(this IServiceCollection services)
        {
            services.AddSingleton<SettingsViewModel, SettingsViewModel>();
            return services;
        }
    }
}
