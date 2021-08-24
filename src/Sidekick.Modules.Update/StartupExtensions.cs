using Microsoft.Extensions.DependencyInjection;
using Sidekick.Modules.Update.Localization;

namespace Sidekick.Modules.Update
{
    public static class StartupExtensions
    {
        public static IServiceCollection AddSidekickUpdate(this IServiceCollection services)
        {
            services.AddTransient<UpdateResources>();

            return services;
        }
    }
}
