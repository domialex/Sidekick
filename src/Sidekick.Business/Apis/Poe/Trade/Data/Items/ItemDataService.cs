using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Sidekick.Core.Initialization;

namespace Sidekick.Business.Apis.Poe.Trade.Data.Items
{
    public class ItemDataService : IItemDataService, IOnInit
    {
        private readonly IPoeApiClient poeApiClient;

        public ItemDataService(IPoeApiClient poeApiClient)
        {
            this.poeApiClient = poeApiClient;
        }

        public List<ItemCategory> Categories { get; private set; }

        private List<(Regex Regex, Item Item)> Patterns { get; set; }

        public async Task OnInit()
        {
            Categories = null;
            Categories = await poeApiClient.Fetch<ItemCategory>();

            Patterns = Categories
                .SelectMany(x => x.Entries)
                .Select(x => (
                    new Regex(Regex.Escape(x.Name ?? x.Text)),
                    x
                ))
                .ToList();
        }

        public Item GetItem(string name)
        {
            var result = Patterns
                .Where(x => x.Regex.IsMatch(name))
                .Select(x => x.Item);

            if (result.Count() <= 1)
            {
                return result.FirstOrDefault();
            }
            else
            {
                return result
                    .Where(x => new Regex(Regex.Escape(x.Text)).IsMatch(name))
                    .FirstOrDefault();
            }
        }
    }
}
