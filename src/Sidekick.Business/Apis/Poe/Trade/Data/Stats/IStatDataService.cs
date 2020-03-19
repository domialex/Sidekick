using Sidekick.Business.Apis.Poe.Models;

namespace Sidekick.Business.Apis.Poe.Trade.Data.Stats
{
    public interface IStatDataService
    {
        Mods ParseMods(string text);
        StatData GetById(string id);
    }
}
