using System.Collections.Generic;
using System.Threading.Tasks;
using Sidekick.Core.Initialization;

namespace Sidekick.Business.Apis.Poe.Trade.Data.Static
{
    public class StaticDataService : IStaticDataService, IOnInit
    {
        private readonly IPoeTradeClient poeApiClient;

        public StaticDataService(IPoeTradeClient poeApiClient)
        {
            this.poeApiClient = poeApiClient;
        }

        private Dictionary<string, string> ImageUrls { get; set; }
        private Dictionary<string, string> Ids { get; set; }

        public async Task OnInit()
        {
            var categories = await poeApiClient.Fetch<StaticItemCategory>();

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
            if (ImageUrls.TryGetValue(text, out var result))
            {
                return result;
            }

            return null;
        }
    }
}
