using Microsoft.Extensions.DependencyInjection;
using Sidekick.Core;
using Sidekick.Services;
using Sidekick.Windows.TrayIcon;

namespace Sidekick
{
    public static class StartupExtensions
    {
        public static IServiceCollection AddSidekickServices(this IServiceCollection services)
        {
            services.AddInitializableService<ITrayService, TrayService>();
            services.AddSingleton<TrayIconViewModel>();

            return services;
        }
    }
}
