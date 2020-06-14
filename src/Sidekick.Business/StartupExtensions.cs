using Microsoft.Extensions.DependencyInjection;
using Sidekick.Business.Apis;
using Sidekick.Business.Apis.Poe.Parser;
using Sidekick.Business.Apis.Poe.Parser.Patterns;
using Sidekick.Business.Apis.Poe.Trade;
using Sidekick.Business.Apis.Poe.Trade.Data.Items;
using Sidekick.Business.Apis.Poe.Trade.Data.Static;
using Sidekick.Business.Apis.Poe.Trade.Data.Stats;
using Sidekick.Business.Apis.Poe.Trade.Data.Stats.Pseudo;
using Sidekick.Business.Apis.Poe.Trade.Leagues;
using Sidekick.Business.Apis.Poe.Trade.Search;
using Sidekick.Business.Apis.PoeDb;
using Sidekick.Business.Apis.PoeNinja;
using Sidekick.Business.Apis.PoePriceInfo.Models;
using Sidekick.Business.Apis.PoeWiki;
using Sidekick.Business.Chat;
using Sidekick.Business.Http;
using Sidekick.Business.Languages;
using Sidekick.Business.Parties;
using Sidekick.Business.Stashes;
using Sidekick.Business.Whispers;
using Sidekick.Business.Windows;
using Sidekick.Core;

namespace Sidekick.Business
{
    public static class StartupExtensions
    {
        public static IServiceCollection AddSidekickBusinessServices(this IServiceCollection services)
        {
            // Http Services
            services.AddHttpClient();

            services.AddSingleton<IChatService, ChatService>();
            services.AddSingleton<IHttpClientProvider, HttpClientProvider>();
            services.AddSingleton<ILanguageProvider, LanguageProvider>();
            services.AddSingleton<IPartyService, PartyService>();
            services.AddSingleton<IPoePriceInfoClient, PoePriceInfoClient>();
            services.AddSingleton<IStashService, StashService>();
            services.AddSingleton<ITradeSearchService, TradeSearchService>();
            services.AddSingleton<IWindowService, WindowService>();
            services.AddSingleton<IWhisperService, WhisperService>();

            services.AddSingleton<IPoeDbClient, PoeDbClient>();
            services.AddSingleton<IPoeWikiClient, PoeWikiClient>();
            services.AddSingleton<IWikiProvider, WikiProviderFactory>();

            services.AddInitializableService<IPoeTradeClient, PoeTradeClient>();
            services.AddInitializableService<IStatDataService, StatDataService>();
            services.AddInitializableService<IItemDataService, ItemDataService>();
            services.AddInitializableService<ILeagueDataService, LeagueDataService>();
            services.AddInitializableService<IParserService, ParserService>();
            services.AddInitializableService<IPoeNinjaClient, PoeNinjaClient>();
            services.AddInitializableService<IPoeNinjaCache, PoeNinjaCache>();
            services.AddInitializableService<IPseudoStatDataService, PseudoStatDataService>();
            services.AddInitializableService<IParserPatterns, ParserPatterns>();
            services.AddInitializableService<IStaticDataService, StaticDataService>();

            return services;
        }
    }
}
