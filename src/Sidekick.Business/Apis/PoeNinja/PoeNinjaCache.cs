using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Sidekick.Business.Apis.PoeNinja.Models;
using Sidekick.Business.Languages.Client;
using Sidekick.Business.Parsers.Models;
using Sidekick.Core.Configuration;
using Sidekick.Core.Initialization;
using Sidekick.Core.Loggers;

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
        private readonly Configuration configuration;

        public DateTime? LastRefreshTimestamp { get; private set; }

        public List<PoeNinjaItem> Items { get; private set; } = new List<PoeNinjaItem>();
        public List<PoeNinjaCurrency> Currencies { get; private set; } = new List<PoeNinjaCurrency>();

        public bool IsInitialized => LastRefreshTimestamp.HasValue;

        public PoeNinjaCache(IPoeNinjaClient client,
                             ILogger logger,
                             Configuration configuration)
        {
            this.client = client;
            this.logger = logger;
            this.configuration = configuration;
        }
        public PoeNinjaItem GetItem(Item item)
        {
            // TODO: Ensure cached items are from the currently selected league (league change needs a few sec to update)
            //if(!IsInitialized)
            //{
            //    throw new Exception("Cache not yet initialized. Call Refresh() before trying to get an item.");
            //}
            return Items.FirstOrDefault(x => x.Name == item.Name);

        }


        /// <summary>
        /// Refreshes the cache with the specified league.
        /// </summary>
        /// <param name="league">The league.</param>
        public async Task OnAfterInit()
        {
            logger.Log($"Fetching PoeNinja cache.");

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

            logger.Log($"PoeNinja cache fetched.");
        }
    }
}
