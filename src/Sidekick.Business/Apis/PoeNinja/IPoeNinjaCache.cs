using System;
using Sidekick.Business.Apis.Poe.Parser;
using Sidekick.Business.Apis.PoeNinja.Models;

namespace Sidekick.Business.Apis.PoeNinja
{
    /// <summary>
    /// poe.ninja cache. The basic idea is fetching current poe.ninja with specified interval (e.g. hourly) in the background.
    /// imo it'd be overkill to request their api every time. Also perfomance. 
    /// Alternatively give the user the option to refresh the cache via TrayIcon or Shortcut.
    /// </summary>
    public interface IPoeNinjaCache
    {
        DateTime? LastRefreshTimestamp { get; }

        PoeNinjaItem GetItem(ParsedItem item);
        PoeNinjaCurrency GetCurrency(ParsedItem item);
        double? GetItemPrice(ParsedItem item);
    }
}
