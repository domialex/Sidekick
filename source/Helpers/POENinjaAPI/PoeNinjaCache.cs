using Sidekick.Helpers.POENinjaAPI.Models;
using Sidekick.Helpers.POETradeAPI.Models.TradeData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sidekick.Helpers.POENinjaAPI
{

    /// <summary>
    /// poe.ninja cache. The basic idea is fetching current poe.ninja with specified interval (e.g. hourly) in the background.
    /// imo it'd be overkill to request their api every time. Also perfomance. 
    /// Alternatively give the user the option to refresh the cache via TrayIcon or Shortcut.
    /// </summary>
    public static class PoeNinjaCache
    {
        public static DateTime? LastRefreshTimestamp { get; private set; }

        public static List<PoeNinjaCacheItem<PoeNinjaItem>> Items { get; private set; } = new List<PoeNinjaCacheItem<PoeNinjaItem>>();

        public static List<PoeNinjaCacheItem<PoeNinjaCurrency>> Currencies { get; private set; } = new List<PoeNinjaCacheItem<PoeNinjaCurrency>>();

        public static League SelectedLeague { get; set; }

        public static bool IsInitialized => LastRefreshTimestamp.HasValue;


        public static PoeNinjaItem GetItem(Item item)
        {
            // TODO: Ensure cached items are from the currently selected league (league change needs a few sec to update)
            //if(!IsInitialized)
            //{
            //    throw new Exception("Cache not yet initialized. Call Refresh() before trying to get an item.");
            //}
            
            return Items.FirstOrDefault(c => c.Type == item.Type)?.Items.FirstOrDefault(c => c.Name == item.Name);
        }


        /// <summary>
        /// Refreshes the cache with the specified league.
        /// </summary>
        /// <param name="league">The league.</param>
        public static async Task Refresh()
        {
            PoeNinjaClient client = new PoeNinjaClient();
            Items = new List<PoeNinjaCacheItem<PoeNinjaItem>>();
            foreach (var itemType in Enum.GetValues(typeof(ItemType)))
            {
                var result = await client.GetItemOverview(SelectedLeague.Id, (ItemType)itemType);
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
                var result = await client.GetCurrencyOverview(SelectedLeague.Id, (CurrencyType)currency);
                PoeNinjaCacheItem<PoeNinjaCurrency> item = new PoeNinjaCacheItem<PoeNinjaCurrency>()
                {
                    Type = currency.ToString(),
                    Items = result.Lines
                };

                Currencies.Add(item);
            }

            LastRefreshTimestamp = DateTime.Now;
            Logger.Log($"poe.ninja cache refreshed for league {SelectedLeague.Id}.");
        }
    }

    public class PoeNinjaCacheItem<T> where T : PoeNinjaResult
    {
        public string Type { get; set; }

        public List<T> Items { get; set; }
    }
}
