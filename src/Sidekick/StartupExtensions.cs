using Microsoft.Extensions.DependencyInjection;
using Sidekick.Services;

namespace Sidekick
{
    public static class StartupExtensions
    {
        public static IServiceCollection AddSidekickServices(this IServiceCollection services)
        {
            services.AddSingleton<ITrayService, TrayService>();

            return services;
        }
    }
}
