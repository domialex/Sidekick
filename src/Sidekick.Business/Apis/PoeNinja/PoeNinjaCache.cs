using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Serilog;
using Sidekick.Business.Apis.Poe.Models;
using Sidekick.Business.Apis.PoeNinja.Models;
using Sidekick.Business.Languages;
using Sidekick.Core.Initialization;
using Sidekick.Core.Settings;

namespace Sidekick.Business.Apis.PoeNinja
{

    /// <summary>
    /// poe.ninja cache.
    /// Fetch poe.ninja with specified interval in the background.
    /// Alternatively give the user the option to refresh the cache via TrayIcon or Shortcut.
    /// </summary>
    public class PoeNinjaCache : IPoeNinjaCache, IOnAfterInit
    {
        private readonly IPoeNinjaClient client;
        private readonly ILogger logger;
        private readonly ILanguageProvider languageProvider;
        private readonly SidekickSettings configuration;

        public DateTime? LastRefreshTimestamp { get; private set; }

        public List<PoeNinjaItem> Items { get; private set; } = new List<PoeNinjaItem>();
        public List<PoeNinjaCurrency> Currencies { get; private set; } = new List<PoeNinjaCurrency>();

        public bool IsInitialized => LastRefreshTimestamp.HasValue;

        public PoeNinjaCache(IPoeNinjaClient client,
                             ILogger logger,
                             ILanguageProvider languageProvider,
                             SidekickSettings configuration)
        {
            this.client = client;
            this.languageProvider = languageProvider;
            this.logger = logger.ForContext(GetType());
            this.configuration = configuration;
        }
        public PoeNinjaItem GetItem(Item item)
        {
            var nameToSearch = item.Type.Contains(languageProvider.Language.KeywordVaal) ? item.Type : item.NameLine;

            var query = Items.Where(x => x.Name == nameToSearch && x.Corrupted == item.Corrupted);

            if (item.Properties.MapTier > 0) query = query.Where(x => x.MapTier == item.Properties.MapTier);

            if (item.Properties.GemLevel > 0) query = query.Where(x => x.GemLevel == item.Properties.GemLevel && x.GemQuality == item.Properties.Quality);

            return query.FirstOrDefault();
        }

        public PoeNinjaCurrency GetCurrency(Item item)
        {
            return Currencies.FirstOrDefault(x => x.CurrencyTypeName == item.NameLine);
        }

        public double? GetItemPrice(Item item)
        {
            return GetCurrency(item)?.Receive.Value ?? GetItem(item)?.ChaosValue;
        }

        /// <summary>
        /// Refreshes the cache with the specified league.
        /// </summary>
        public async Task OnAfterInit()
        {
            logger.Information($"Populating PoeNinja cache.");

            var itemsTasks = Enum.GetValues(typeof(ItemType))
                                 .Cast<ItemType>()
                                 .Select(x => new { itemType = x, request = client.QueryItem(configuration.LeagueId, x) })
                                 .ToList();
            var currenciesTasks = Enum.GetValues(typeof(CurrencyType))
                                      .Cast<CurrencyType>()
                                      .Select(x => new { currencyType = x, request = client.QueryItem(configuration.LeagueId, x) })
                                      .ToList();

            await Task.WhenAll(itemsTasks.Select(x => x.request).Cast<Task>().Concat(currenciesTasks.Select(x => x.request).Cast<Task>()));

            Items.AddRange(itemsTasks.Select(x => new PoeNinjaCacheItem<PoeNinjaItem> { Type = x.itemType.ToString(), Items = x.request.Result.Lines }).SelectMany(x => x.Items));
            Currencies.AddRange(currenciesTasks.Select(x => new PoeNinjaCacheItem<PoeNinjaCurrency> { Type = x.currencyType.ToString(), Items = x.request.Result.Lines }).SelectMany(x => x.Items));

            LastRefreshTimestamp = DateTime.Now;

            logger.Information($"PoeNinja cache populated.");
        }
    }
}
