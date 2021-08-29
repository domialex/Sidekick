using Microsoft.Extensions.DependencyInjection;
using Sidekick.Common.Blazor;
using Sidekick.Modules.Update.Localization;

namespace Sidekick.Modules.Update
{
    public static class StartupExtensions
    {
        public static IServiceCollection AddSidekickUpdate(this IServiceCollection services)
        {
            services.AddTransient<UpdateResources>();
            services.AddSidekickModule(new SidekickModule()
            {
                Assembly = typeof(StartupExtensions).Assembly
            });

            return services;
        }
    }
}
