using Microsoft.Extensions.DependencyInjection;
using Sidekick.Business.Apis.Poe;
using Sidekick.Business.Apis.PoeDb;
using Sidekick.Business.Apis.PoePriceInfo.Models;
using Sidekick.Business.Apis.PoeWiki;
using Sidekick.Business.Categories;
using Sidekick.Business.Http;
using Sidekick.Business.Languages.Client;
using Sidekick.Business.Languages.UI;
using Sidekick.Business.Leagues;
using Sidekick.Business.Maps;
using Sidekick.Business.Parsers;
using Sidekick.Business.Tokenizers;
using Sidekick.Business.Tokenizers.ItemName;
using Sidekick.Business.Trades;
using Sidekick.Core;

namespace Sidekick.Business
{
    public static class StartupExtensions
    {
        public static IServiceCollection AddSidekickBusinessServices(this IServiceCollection services)
        {
            // Http Services
            services.AddHttpClient();

            services.AddSingleton<IHttpClientProvider, HttpClientProvider>();
            services.AddSingleton<IItemParser, ItemParser>();
            services.AddSingleton<ILanguageProvider, LanguageProvider>();
            services.AddSingleton<IPoeDbClient, PoeDbClient>();
            services.AddSingleton<IPoePriceInfoClient, PoePriceInfoClient>();
            services.AddSingleton<IPoeWikiClient, PoeWikiClient>();
            services.AddSingleton<ITokenizer, ItemNameTokenizer>();
            services.AddSingleton<ITradeClient, TradeClient>();
            services.AddSingleton<IUILanguageProvider, UILanguageProvider>();

            services.AddInitializableService<IPoeApiClient, PoeApiClient>();
            services.AddInitializableService<IAttributeCategoryService, AttributeCategoryService>();
            services.AddInitializableService<IItemCategoryService, ItemCategoryService>();
            services.AddInitializableService<ILeagueService, LeagueService>();
            services.AddInitializableService<IMapService, MapService>();
            services.AddInitializableService<IStaticItemCategoryService, StaticItemCategoryService>();

            return services;
        }
    }
}
