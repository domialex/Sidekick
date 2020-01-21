using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Sidekick.Business.Apis.PoeNinja.Models;
using Sidekick.Business.Parsers.Models;
using Sidekick.Core.Configuration;
using Sidekick.Core.Initialization;

namespace Sidekick.Business.Apis.PoeNinja
{

    /// <summary>
    /// poe.ninja cache.
    /// Fetch poe.ninja with specified interval in the background.
    /// Alternatively give the user the option to refresh the cache via TrayIcon or Shortcut.
    /// </summary>
    public class PoeNinjaCache : IPoeNinjaCache, IOnAfterInit
    {
        private readonly IPoeNinjaClient _client;
        private readonly Configuration _configuration;

        public DateTime? LastRefreshTimestamp { get; private set; }

        public List<PoeNinjaCacheItem<PoeNinjaItem>> Items { get; private set; } = new List<PoeNinjaCacheItem<PoeNinjaItem>>();

        public List<PoeNinjaCacheItem<PoeNinjaCurrency>> Currencies { get; private set; } = new List<PoeNinjaCacheItem<PoeNinjaCurrency>>();

        public bool IsInitialized => LastRefreshTimestamp.HasValue;

        private List<PoeNinjaItem> FlatItems => Items.SelectMany(x => x.Items).ToList();
        private List<PoeNinjaItem> FlatCurrencies => Items.SelectMany(x => x.Items).ToList();

        public PoeNinjaCache(IPoeNinjaClient client, Configuration configuration)
        {
            _client = client;
            _configuration = configuration;
        }
        public PoeNinjaItem GetItem(Item item)
        {
            // TODO: Ensure cached items are from the currently selected league (league change needs a few sec to update)
            //if(!IsInitialized)
            //{
            //    throw new Exception("Cache not yet initialized. Call Refresh() before trying to get an item.");
            //}
            return FlatItems.FirstOrDefault(c => c.Name == item.Name);
        }


        /// <summary>
        /// Refreshes the cache with the specified league.
        /// </summary>
        /// <param name="league">The league.</param>
        public async Task Refresh()
        {
            Items = new List<PoeNinjaCacheItem<PoeNinjaItem>>();
            foreach (var itemType in Enum.GetValues(typeof(ItemType)).Cast<ItemType>())
            {
                var result = await _client.QueryItem(_configuration.LeagueId, itemType);
                if (result != null)
                {
                    var item = new PoeNinjaCacheItem<PoeNinjaItem>()
                    {
                        Type = itemType.ToString(),
                        Items = result.Lines
                    };

                    Items.Add(item);
                }
            }

            Currencies = new List<PoeNinjaCacheItem<PoeNinjaCurrency>>();
            foreach (var currency in Enum.GetValues(typeof(CurrencyType)).Cast<CurrencyType>())
            {
                var result = await _client.QueryItem(_configuration.LeagueId, currency);
                if (result != null)
                {
                    var item = new PoeNinjaCacheItem<PoeNinjaCurrency>()
                    {
                        Type = currency.ToString(),
                        Items = result.Lines
                    };

                    Currencies.Add(item);
                }
            }

            LastRefreshTimestamp = DateTime.Now;
        }

        public async Task OnAfterInit()
        {
            await Refresh();
        }
    }

    public class PoeNinjaCacheItem<T> where T : PoeNinjaResult
    {
        public string Type { get; set; }

        public List<T> Items { get; set; }
    }
}
