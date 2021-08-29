using Microsoft.Extensions.DependencyInjection;
using Sidekick.Common.Blazor;
using Sidekick.Modules.Trade.Localization;

namespace Sidekick.Modules.Trade
{
    public static class StartupExtensions
    {
        public static IServiceCollection AddSidekickTrade(this IServiceCollection services)
        {
            services.AddSidekickModule(new SidekickModule()
            {
                Assembly = typeof(StartupExtensions).Assembly,
            });

            services.AddTransient<TradeResources>();
            services.AddTransient<PoeNinjaResources>();

            return services;
        }
    }
}
