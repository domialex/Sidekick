using Microsoft.Extensions.DependencyInjection;
using Sidekick.Core;
using Sidekick.Services;

namespace Sidekick
{
    public static class StartupExtensions
    {
        public static IServiceCollection AddSidekickServices(this IServiceCollection services)
        {
            services.AddInitializableService<ITrayService, TrayService>();

            return services;
        }
    }
}
