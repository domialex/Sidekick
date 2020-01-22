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
        public static void Initialize()
        {
            InitializeService = Program.ServiceProvider.GetService<IInitializer>();
            Configuration = Program.ServiceProvider.GetService<Configuration>();
            Logger = Program.ServiceProvider.GetService<ILogger>();
            TradeClient = Program.ServiceProvider.GetService<ITradeClient>();
            LanguageProvider = Program.ServiceProvider.GetService<ILanguageProvider>();
            ItemParser = Program.ServiceProvider.GetService<IItemParser>();
            HttpClientProvider = Program.ServiceProvider.GetService<IHttpClientProvider>();
            LeagueService = Program.ServiceProvider.GetService<ILeagueService>();
            UILanguageProvider = Program.ServiceProvider.GetService<IUILanguageProvider>();
            PoeNinjaCache = Program.ServiceProvider.GetService<IPoeNinjaCache>();
            PoeDbClient = Program.ServiceProvider.GetService<IPoeDbClient>();
            PoePriceInfoClient = Program.ServiceProvider.GetService<IPoePriceInfoClient>();
            PoeWikiClient = Program.ServiceProvider.GetService<IPoeWikiClient>();
        }
    }
}
