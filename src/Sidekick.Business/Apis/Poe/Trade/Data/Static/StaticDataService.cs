using System.Collections.Generic;
using Sidekick.Business.Apis.Poe.Models;

namespace Sidekick.Business.Apis.Poe.Trade.Data.Static
{
    public class StaticDataService : IStaticDataService
    {
        public Dictionary<string, string> ImageUrls { get; set; }
        public Dictionary<string, string> Ids { get; set; }

        public string GetImage(string id)
        {
            if (ImageUrls.TryGetValue(id, out var result))
            {
                return result;
            }

            return null;
        }

        public string GetId(string text)
        {
            if (Ids.TryGetValue(text, out var result))
            {
                return result;
            }

            return null;
        }

        public string GetId(Item item) => GetId(item.Name ?? item.Type);
    }
}
