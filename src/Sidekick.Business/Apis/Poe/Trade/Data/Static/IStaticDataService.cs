using System.Collections.Generic;
using Sidekick.Business.Apis.Poe.Models;

namespace Sidekick.Business.Apis.Poe.Trade.Data.Static
{
    public interface IStaticDataService
    {
        Dictionary<string, string> ImageUrls { get; set; }
        Dictionary<string, string> Ids { get; set; }
        string GetImage(string id);
        string GetId(string text);
        string GetId(Item item);
    }
}
