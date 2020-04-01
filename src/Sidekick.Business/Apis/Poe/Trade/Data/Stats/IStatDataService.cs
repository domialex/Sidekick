using Sidekick.Business.Apis.Poe.Models;

namespace Sidekick.Business.Apis.Poe.Trade.Data.Stats
{
    public interface IStatDataService
    {
        Modifiers ParseMods(string text);
        StatData GetById(string id);
    }
}
