using System.Collections.Generic;
using Sidekick.Business.Apis.Poe.Models;

namespace Sidekick.Business.Apis.Poe.Trade.Data.Stats
{
    public interface IStatDataService
    {
        List<StatDataCategory> Categories { get; }
        Mods GetMods(string text);
    }
}
