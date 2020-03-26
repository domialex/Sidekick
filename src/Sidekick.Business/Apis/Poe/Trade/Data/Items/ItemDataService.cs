using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Sidekick.Core.Initialization;

namespace Sidekick.Business.Apis.Poe.Trade.Data.Items
{
    public class ItemDataService : IItemDataService, IOnInit
    {
        private readonly IPoeTradeClient poeApiClient;

        public ItemDataService(IPoeTradeClient poeApiClient)
        {
            this.poeApiClient = poeApiClient;
        }

        private List<(Regex Regex, ItemData Item)> NamePatterns { get; set; }

        private List<(Regex Regex, ItemData Item)> TypePatterns { get; set; }

        public async Task OnInit()
        {
            var categories = await poeApiClient.Fetch<ItemDataCategory>();
            var patterns = categories.SelectMany(x => x.Entries);

            TypePatterns = patterns
                .Where(x => string.IsNullOrEmpty(x.Name))
                .Select(x => (
                    Regex: new Regex($"[\\ \\r\\n]{Regex.Escape(x.Type)}[\\ \\r\\n]"),
                    Item: x
                ))
                .ToList();

            NamePatterns = patterns
                .Where(x => !string.IsNullOrEmpty(x.Name))
                .Select(x => (
                    Regex: new Regex($"[\\ \\r\\n]{Regex.Escape(x.Name)}[\\ \\r\\n]"),
                    Item: x
                ))
                .ToList();
        }

        public ItemData GetItem(string text)
        {
            var items = NamePatterns
                .Where(x => x.Regex.IsMatch(text))
                .Select(x => new
                {
                    x.Item,
                    x.Regex.Match(text).Index,
                })
                .OrderBy(x => x.Index)
                .ThenBy(x => x.Item.Name.Length)
                .Select(x => x.Item)
                .ToList();

            if (items.Count != 0)
            {
                return items[0];
            }

            items = TypePatterns
                .Where(x => x.Regex.IsMatch(text))
                .Select(x => new
                {
                    x.Item,
                    x.Regex.Match(text).Index,
                })
                .OrderBy(x => x.Index)
                .ThenBy(x => x.Item.Type.Length)
                .Select(x => x.Item)
                .ToList();

            return items.FirstOrDefault();
        }
    }
}
