using System.Collections.Generic;

namespace Sidekick.Business.Apis.Poe.Trade.Data.Static
{
    public interface IStaticDataService
    {
        List<StaticItemCategory> Categories { get; }
        Dictionary<string, string> Lookup { get; }
        string GetImage(string id);
    }
}