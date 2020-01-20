using Microsoft.Extensions.Logging;
using Sidekick.Business.Leagues.Models;
using Sidekick.Business.Trades.Results;
using Sidekick.Core.Initialization;
using Sidekick.Business.Apis.PoeNinja.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sidekick.Business.Parsers.Models;

namespace Sidekick.Business.Apis.PoeNinja
{

    /// <summary>
    /// poe.ninja cache. The basic idea is fetching current poe.ninja with specified interval (e.g. hourly) in the background.
    /// imo it'd be overkill to request their api every time. Also perfomance. 
    /// Alternatively give the user the option to refresh the cache via TrayIcon or Shortcut.
    /// </summary>
    public class PoeNinjaCache : IPoeNinjaCache, IOnBeforeInit
    {
        private readonly PoeNinjaClient _client;

        public DateTime? LastRefreshTimestamp { get; private set; }

        public List<PoeNinjaCacheItem<PoeNinjaItem>> Items { get; private set; } = new List<PoeNinjaCacheItem<PoeNinjaItem>>();

        public List<PoeNinjaCacheItem<PoeNinjaCurrency>> Currencies { get; private set; } = new List<PoeNinjaCacheItem<PoeNinjaCurrency>>();

        public League SelectedLeague { get; set; }

        public bool IsInitialized => LastRefreshTimestamp.HasValue;

        private List<PoeNinjaItem> FlatItems => Items.SelectMany(c => c.Items).ToList();
        private List<PoeNinjaItem> FlatCurrencies => Items.SelectMany(c => c.Items).ToList();

        public PoeNinjaCache(PoeNinjaClient client)
        {
            _client = client;
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
            foreach (var itemType in Enum.GetValues(typeof(ItemType)))
            {
                var result = await _client.GetItemOverview(SelectedLeague.Id, (ItemType)itemType);
                PoeNinjaCacheItem<PoeNinjaItem> item = new PoeNinjaCacheItem<PoeNinjaItem>()
                {
                    Type = itemType.ToString(),
                    Items = result.Lines
                };

                Items.Add(item);
            }

            Currencies = new List<PoeNinjaCacheItem<PoeNinjaCurrency>>();

            foreach(var currency in Enum.GetValues(typeof(CurrencyType)))
            {
                var result = await _client.GetCurrencyOverview(SelectedLeague.Id, (CurrencyType)currency);
                PoeNinjaCacheItem<PoeNinjaCurrency> item = new PoeNinjaCacheItem<PoeNinjaCurrency>()
                {
                    Type = currency.ToString(),
                    Items = result.Lines
                };

                Currencies.Add(item);
            }

            LastRefreshTimestamp = DateTime.Now;
            //_logger.LogInformation($"poe.ninja cache refreshed for league {SelectedLeague.Id}.");
        }

        public async Task OnBeforeInit()
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
