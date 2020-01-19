using Microsoft.Extensions.DependencyInjection;
using Sidekick.Business.Apis.Poe;
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

namespace Sidekick.Business
{
    public static class StartupExtensions
    {
        public static IServiceCollection AddSidekickBusinessServices(this IServiceCollection services)
        {
            // Http Services
            services.AddHttpClient();

            services.AddSingleton<IAttributeCategoryService, AttributeCategoryService>();
            services.AddSingleton<IHttpClientProvider, HttpClientProvider>();
            services.AddSingleton<IItemCategoryService, ItemCategoryService>();
            services.AddSingleton<IItemParser, ItemParser>();
            services.AddSingleton<ILanguageProvider, LanguageProvider>();
            services.AddSingleton<ILeagueService, LeagueService>();
            services.AddSingleton<IMapService, MapService>();
            services.AddSingleton<IPoeApiService, PoeApiService>();
            services.AddSingleton<IStaticItemCategoryService, StaticItemCategoryService>();
            services.AddSingleton<ITokenizer, ItemNameTokenizer>();
            services.AddSingleton<ITradeClient, TradeClient>();
            services.AddSingleton<IUILanguageProvider, UILanguageProvider>();

            return services;
        }
    }
}
