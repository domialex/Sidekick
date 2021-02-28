using System.Collections.Generic;
using System.Threading.Tasks;
using Sidekick.Domain.Cache;
using Sidekick.Domain.Game.Items.Metadatas;
using Sidekick.Domain.Game.Items.Models;
using Sidekick.Infrastructure.PoeApi.Items.Static.Models;

namespace Sidekick.Infrastructure.PoeApi.Items.Static
{
    public class ItemStaticDataProvider : IItemStaticDataProvider
    {
        private readonly ICacheRepository cacheRepository;
        private readonly IPoeTradeClient poeTradeClient;

        public ItemStaticDataProvider(
            ICacheRepository cacheRepository,
            IPoeTradeClient poeTradeClient)
        {
            this.cacheRepository = cacheRepository;
            this.poeTradeClient = poeTradeClient;
        }

        private Dictionary<string, string> ImageUrls { get; set; }
        private Dictionary<string, string> Ids { get; set; }

        public async Task Initialize()
        {
            var result = await cacheRepository.GetOrSet(
                "Sidekick.Infrastructure.PoeApi.Items.Static.ItemStaticDataProvider.Initialize",
                () => poeTradeClient.Fetch<StaticItemCategory>("data/static"));

            ImageUrls = new Dictionary<string, string>();
            Ids = new Dictionary<string, string>();
            foreach (var category in result.Result)
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

        public string GetId(Item item) => GetId(item.Metadata.Name ?? item.Metadata.Type);
    }
}
