using System.Collections.Generic;
using System.Linq;
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

        public List<StaticItemCategory> Categories { get; private set; }
        public Dictionary<string, string> Lookup { get; private set; }
        private Dictionary<string, string> ImageUrls { get; set; }

        public async Task OnInit()
        {
            Categories = null;
            Categories = await poeApiClient.Fetch<StaticItemCategory>();
            Lookup = ToLookup();
            FillImages();
        }

        private Dictionary<string, string> ToLookup()
        {
            return Categories.Where(x => !x.Id.StartsWith("Maps")).SelectMany(x => x.Entries).ToDictionary(key => key.Text, value => value.Id);
        }

        private void FillImages()
        {
            ImageUrls = new Dictionary<string, string>();

            foreach (var category in Categories)
            {
                foreach (var entry in category.Entries)
                {
                    ImageUrls.Add(entry.Id, entry.Image);
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
    }
}
