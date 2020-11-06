using Microsoft.Extensions.DependencyInjection;
using Sidekick.Business.Apis;
using Sidekick.Business.Apis.Poe.Parser;
using Sidekick.Business.Apis.Poe.Parser.Patterns;
using Sidekick.Business.Apis.Poe.Trade;
using Sidekick.Business.Apis.Poe.Trade.Data.Items;
using Sidekick.Business.Apis.Poe.Trade.Data.Static;
using Sidekick.Business.Apis.Poe.Trade.Data.Stats;
using Sidekick.Business.Apis.Poe.Trade.Data.Stats.Pseudo;
using Sidekick.Business.Apis.Poe.Trade.Search;
using Sidekick.Business.Apis.PoeDb;
using Sidekick.Business.Apis.PoeNinja;
using Sidekick.Business.Apis.PoePriceInfo.Models;
using Sidekick.Business.Apis.PoeWiki;
using Sidekick.Business.Http;
using Sidekick.Business.ItemCategories;
using Sidekick.Business.Languages;
using Sidekick.Business.Parties;
using Sidekick.Business.Whispers;
using Sidekick.Business.Windows;
using Sidekick.Domain.Languages;

namespace Sidekick.Business
{
    public static class StartupExtensions
    {
        public static IServiceCollection AddSidekickBusinessServices(this IServiceCollection services)
        {
            // Http Services
            services.AddHttpClient();

            services.AddSingleton<IHttpClientProvider, HttpClientProvider>();
            services.AddSingleton<IItemCategoryService, ItemCategoryService>();
            services.AddSingleton<ILanguageProvider, LanguageProvider>();
            services.AddSingleton<IPartyService, PartyService>();
            services.AddSingleton<IPoePriceInfoClient, PoePriceInfoClient>();
            services.AddSingleton<ITradeSearchService, TradeSearchService>();
            services.AddSingleton<IWindowService, WindowService>();
            services.AddSingleton<IWhisperService, WhisperService>();

            services.AddSingleton<IPoeDbClient, PoeDbClient>();
            services.AddSingleton<IPoeWikiClient, PoeWikiClient>();
            services.AddSingleton<IWikiProvider, WikiProviderFactory>();

            services.AddSingleton<IPoeTradeClient, PoeTradeClient>();
            services.AddSingleton<IStatDataService, StatDataService>();
            services.AddSingleton<IItemDataService, ItemDataService>();
            services.AddSingleton<IParserService, ParserService>();
            services.AddSingleton<IPoeNinjaClient, PoeNinjaClient>();
            services.AddSingleton<IPoeNinjaCache, PoeNinjaCache>();
            services.AddSingleton<IPseudoStatDataService, PseudoStatDataService>();
            services.AddSingleton<IParserPatterns, ParserPatterns>();
            services.AddSingleton<IStaticDataService, StaticDataService>();

            return services;
        }
    }
}
