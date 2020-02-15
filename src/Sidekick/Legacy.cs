using System;
using Microsoft.Extensions.DependencyInjection;
using Sidekick.Business.Apis.PoeDb;
using Sidekick.Business.Apis.PoeNinja;
using Sidekick.Business.Apis.PoePriceInfo.Models;
using Sidekick.Business.Apis.PoeWiki;
using Sidekick.Business.Languages;
using Sidekick.Business.Parsers;
using Sidekick.Business.Trades;
using Sidekick.Business.Whispers;
using Sidekick.Core.Loggers;
using Sidekick.Core.Natives;
using Sidekick.Core.Settings;
using Sidekick.Localization;
using Sidekick.UI.Views;
//
namespace Sidekick
{
    [Obsolete]
    public static class Legacy
    {
//         [Obsolete]
//         public static SidekickSettings Settings { get; private set; }
//
         [Obsolete]
         public static ILogger Logger { get; private set; }
//
//         [Obsolete]
//         public static ITradeClient TradeClient { get; private set; }
//
         [Obsolete]
         public static ILanguageProvider LanguageProvider { get; private set; }
//
         // [Obsolete]
         // public static IUILanguageProvider UILanguageProvider { get; private set; }
//
//         [Obsolete]
//         public static IItemParser ItemParser { get; private set; }
//
//         [Obsolete]
//         public static IPoeNinjaCache PoeNinjaCache { get; private set; }
//
//         [Obsolete]
//         public static IPoeDbClient PoeDbClient { get; private set; }
//
//         [Obsolete]
//         public static IPoePriceInfoClient PoePriceInfoClient { get; private set; }
//
//         [Obsolete]
//         public static IPoeWikiClient PoeWikiClient { get; private set; }
//
//         [Obsolete]
//         public static IKeybindEvents KeybindEvents { get; private set; }
//
//         [Obsolete]
//         public static INativeProcess NativeProcess { get; private set; }
//
//         [Obsolete]
//         public static INativeKeyboard NativeKeyboard { get; private set; }
//
//         [Obsolete]
//         public static INativeClipboard NativeClipboard { get; private set; }
//
//         [Obsolete]
//         public static INativeBrowser NativeBrowser { get; private set; }
//
//         //[Obsolete]
//        // public static IViewLocator ViewLocator { get; private set; }
//
//         [Obsolete]
//         public static IWhisperService WhisperService { get; private set; }
//
         [Obsolete]
         public static void Initialize(IServiceProvider serviceProvider)
         {
             // Settings = serviceProvider.GetService<SidekickSettings>();
              Logger = serviceProvider.GetService<ILogger>();
             // TradeClient = serviceProvider.GetService<ITradeClient>();
             LanguageProvider = serviceProvider.GetService<ILanguageProvider>();
             // ItemParser = serviceProvider.GetService<IItemParser>();
             // UILanguageProvider = serviceProvider.GetService<IUILanguageProvider>();
             // PoeNinjaCache = serviceProvider.GetService<IPoeNinjaCache>();
             // PoeDbClient = serviceProvider.GetService<IPoeDbClient>();
             // PoePriceInfoClient = serviceProvider.GetService<IPoePriceInfoClient>();
             // PoeWikiClient = serviceProvider.GetService<IPoeWikiClient>();
             // KeybindEvents = serviceProvider.GetService<IKeybindEvents>();
             // NativeProcess = serviceProvider.GetService<INativeProcess>();
             // NativeKeyboard = serviceProvider.GetService<INativeKeyboard>();
             // NativeClipboard = serviceProvider.GetService<INativeClipboard>();
             // NativeBrowser = serviceProvider.GetService<INativeBrowser>();
             // // ViewLocator = serviceProvider.GetService<IViewLocator>();
             // WhisperService = serviceProvider.GetService<IWhisperService>();
         }
     }
}
