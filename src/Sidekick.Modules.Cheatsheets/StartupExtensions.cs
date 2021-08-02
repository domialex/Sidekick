using Microsoft.Extensions.DependencyInjection;
using Sidekick.Common.Blazor;
using Sidekick.Modules.Cheatsheets.Localization;

namespace Sidekick.Modules.Cheatsheets
{
    public static class StartupExtensions
    {
        public static IServiceCollection AddSidekickCheatsheets(this IServiceCollection services)
        {
            services.AddSidekickModule(typeof(StartupExtensions).Assembly);

            services.AddTransient<BetrayalResources>();
            services.AddTransient<BlightResources>();
            services.AddTransient<CheatsheetResources>();
            services.AddTransient<DelveResources>();
            services.AddTransient<HeistResources>();
            services.AddTransient<IncursionResources>();
            services.AddTransient<MetamorphResources>();

            return services;
        }
    }
}
