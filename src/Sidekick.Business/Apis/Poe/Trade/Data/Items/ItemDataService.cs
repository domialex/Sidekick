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
                    Regex: new Regex(Regex.Escape(x.Type)),
                    Item: x
                ))
                .ToList();

            NamePatterns = patterns
                .Where(x => !string.IsNullOrEmpty(x.Name))
                .Select(x => (
                    Regex: new Regex(Regex.Escape(x.Name)),
                    Item: x
                ))
                .ToList();
        }

        public ItemData GetItem(string text)
        {
            var result = NamePatterns
                .Where(x => x.Regex.IsMatch(text));

            if (result.Count() != 0)
            {
                return result
                    .Select(x => x.Item)
                    .OrderByDescending(x => x.Name.Length)
                    .FirstOrDefault();
            }

            return TypePatterns
                .Where(x => x.Regex.IsMatch(text))
                .Select(x => x.Item)
                .OrderByDescending(x => x.Type.Length)
                .FirstOrDefault();
        }
    }
}
