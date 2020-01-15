using Microsoft.Extensions.DependencyInjection;
using Sidekick.Business.Http;
using Sidekick.Business.Languages;
using Sidekick.Business.Loggers;
using Sidekick.Business.Notifications;
using Sidekick.Business.Parsers;
using Sidekick.Business.Trades;
using System;

namespace Sidekick
{
    [Obsolete]
    public static class Legacy
    {
        [Obsolete]
        public static ILogger Logger { get; private set; }

        [Obsolete]
        public static ITradeClient TradeClient { get; private set; }

        [Obsolete]
        public static ILanguageProvider LanguageProvider { get; private set; }

        [Obsolete]
        public static INotificationService NotificationService { get; private set; }

        [Obsolete]
        public static IItemParser ItemParser { get; private set; }

        [Obsolete]
        public static IHttpClientProvider HttpClientProvider { get; private set; }

        [Obsolete]
        public static void Initialize()
        {
            Logger = Program.ServiceProvider.GetService<ILogger>();
            TradeClient = Program.ServiceProvider.GetService<ITradeClient>();
            LanguageProvider = Program.ServiceProvider.GetService<ILanguageProvider>();
            NotificationService = Program.ServiceProvider.GetService<INotificationService>();
            ItemParser = Program.ServiceProvider.GetService<IItemParser>();
            HttpClientProvider = Program.ServiceProvider.GetService<IHttpClientProvider>();
        }
    }
}
