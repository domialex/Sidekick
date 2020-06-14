using System.Collections.Generic;
using System.Threading.Tasks;
using Sidekick.Business.Apis.Poe.Models;
using Sidekick.Business.Caches;
using Sidekick.Core.Initialization;

namespace Sidekick.Business.Apis.Poe.Trade.Data.Static
{
    public class StaticDataService : IStaticDataService, IOnInit
    {
        private readonly IPoeTradeClient poeApiClient;
        private readonly ICacheService cacheService;

        public StaticDataService(IPoeTradeClient poeApiClient,
            ICacheService cacheService)
        {
            this.poeApiClient = poeApiClient;
            this.cacheService = cacheService;
        }

        private Dictionary<string, string> ImageUrls { get; set; }
        private Dictionary<string, string> Ids { get; set; }

        public async Task OnInit()
        {
            var categories = await cacheService.Get<List<StaticItemCategory>>("StaticDataService.OnInit");
            if (categories == default)
            {
                categories = await poeApiClient.Fetch<StaticItemCategory>();
                await cacheService.Save("StaticDataService.OnInit", categories);
            }

            ImageUrls = new Dictionary<string, string>();
            Ids = new Dictionary<string, string>();
            foreach (var category in categories)
            {
                foreach (var entry in category.Entries)
                {
                    ImageUrls.Add(entry.Id, entry.Image);
                    if (!Ids.ContainsKey(entry.Text))
                    {
                        Ids.Add(entry.Text, entry.Id);
                    }
                }
            }
        }

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
