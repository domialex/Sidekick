using System;
using Microsoft.Extensions.DependencyInjection;
using Sidekick.Business.Apis.PoeDb;
using Sidekick.Business.Apis.PoeNinja;
using Sidekick.Business.Apis.PoePriceInfo.Models;
using Sidekick.Business.Apis.PoeWiki;
using Sidekick.Business.Http;
using Sidekick.Business.Languages.Client;
using Sidekick.Business.Languages.UI;
using Sidekick.Business.Leagues;
using Sidekick.Business.Parsers;
using Sidekick.Business.Trades;
using Sidekick.Core.Configuration;
using Sidekick.Core.Initialization;
using Sidekick.Core.Loggers;

namespace Sidekick
{
    [Obsolete]
    public static class Legacy
    {
        [Obsolete]
        public static IInitializer InitializeService { get; private set; }

        [Obsolete]
        public static Configuration Configuration { get; private set; }

        [Obsolete]
        public static ILogger Logger { get; private set; }

        [Obsolete]
        public static ITradeClient TradeClient { get; private set; }

        [Obsolete]
        public static ILanguageProvider LanguageProvider { get; private set; }

        [Obsolete]
        public static IUILanguageProvider UILanguageProvider { get; private set; }

        [Obsolete]
        public static IItemParser ItemParser { get; private set; }

        [Obsolete]
        public static IHttpClientProvider HttpClientProvider { get; private set; }

        [Obsolete]
        public static ILeagueService LeagueService { get; private set; }

        [Obsolete]
        public static IPoeNinjaCache PoeNinjaCache { get; private set; }

        [Obsolete]
        public static IPoeDbClient PoeDbClient { get; private set; }

        [Obsolete]
        public static IPoePriceInfoClient PoePriceInfoClient { get; private set; }

        [Obsolete]
        public static IPoeWikiClient PoeWikiClient { get; private set; }

        [Obsolete]
        public static void Initialize(IServiceProvider serviceProvider)
        {
            InitializeService = serviceProvider.GetService<IInitializer>();
            Configuration = serviceProvider.GetService<Configuration>();
            Logger = serviceProvider.GetService<ILogger>();
            TradeClient = serviceProvider.GetService<ITradeClient>();
            LanguageProvider = serviceProvider.GetService<ILanguageProvider>();
            ItemParser = serviceProvider.GetService<IItemParser>();
            HttpClientProvider = serviceProvider.GetService<IHttpClientProvider>();
            LeagueService = serviceProvider.GetService<ILeagueService>();
            UILanguageProvider = serviceProvider.GetService<IUILanguageProvider>();
            PoeNinjaCache = serviceProvider.GetService<IPoeNinjaCache>();
            PoeDbClient = serviceProvider.GetService<IPoeDbClient>();
            PoePriceInfoClient = serviceProvider.GetService<IPoePriceInfoClient>();
            PoeWikiClient = serviceProvider.GetService<IPoeWikiClient>();
        }
    }
}
