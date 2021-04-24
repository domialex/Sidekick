using Microsoft.Extensions.DependencyInjection;
using Sidekick.Domain.Game.Items.Metadatas;
using Sidekick.Domain.Game.Items.Modifiers;
using Sidekick.Domain.Game.Modifiers;
using Sidekick.Domain.Game.Trade;
using Sidekick.Infrastructure.Github;
using Sidekick.Infrastructure.PoeApi;
using Sidekick.Infrastructure.PoeApi.Items.Metadatas;
using Sidekick.Infrastructure.PoeApi.Items.Modifiers;
using Sidekick.Infrastructure.PoeApi.Items.Pseudo;
using Sidekick.Infrastructure.PoeApi.Items.Static;
using Sidekick.Infrastructure.PoeApi.Trade;
using Sidekick.Infrastructure.PoeNinja;
using Sidekick.Infrastructure.PoePriceInfo;
using Sidekick.Infrastructure.RePoe.Data.StatTranslations;

namespace Sidekick.Infrastructure
{
    public static class StartupExtensions
    {
        public static IServiceCollection AddSidekickInfrastructure(this IServiceCollection services)
        {
            services.AddHttpClient();

            services.AddTransient<IPoeTradeClient, PoeTradeClient>();
            services.AddTransient<IGithubClient, GithubClient>();
            services.AddTransient<IPoePriceInfoClient, PoePriceInfoClient>();
            services.AddTransient<IPoeNinjaClient, PoeNinjaClient>();

            // PoeApi
            services.AddSingleton<ITradeSearchService, TradeSearchService>();
            services.AddSingleton<IItemMetadataProvider, ItemMetadataProvider>();
            services.AddSingleton<IModifierProvider, ModifierProvider>();
            services.AddSingleton<IPseudoModifierProvider, PseudoModifierProvider>();
            services.AddSingleton<IItemStaticDataProvider, ItemStaticDataProvider>();

            // RePoe
            services.AddSingleton<IStatTranslationProvider, StatTranslationProvider>();

            return services;
        }
    }
}
