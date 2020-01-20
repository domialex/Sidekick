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
    public interface IPoeNinjaCache
    {
        DateTime? LastRefreshTimestamp { get; }

        Task Refresh();

        PoeNinjaItem GetItem(Item item);

    }
}
