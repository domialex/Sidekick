using Microsoft.Extensions.DependencyInjection;
using Sidekick.Domain.Game.Trade;
using Sidekick.Infrastructure.PoeApi;
using Sidekick.Infrastructure.PoeApi.Trade;
using Sidekick.Infrastructure.PoePriceInfo;

namespace Sidekick.Infrastructure
{
    public static class StartupExtensions
    {
        public static IServiceCollection AddSidekickInfrastructure(this IServiceCollection services)
        {
            services.AddHttpClient();

            services.AddTransient<IPoeTradeClient, PoeTradeClient>();
            services.AddTransient<IPoePriceInfoClient, PoePriceInfoClient>();

            // PoeApi
            services.AddSingleton<ITradeSearchService, TradeSearchService>();

            return services;
        }
    }
}
