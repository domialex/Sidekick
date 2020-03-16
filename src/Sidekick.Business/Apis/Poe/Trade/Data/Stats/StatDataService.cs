using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Sidekick.Core.Initialization;

namespace Sidekick.Business.Apis.Poe.Trade.Data.Stats
{
    public class StatDataService : IStatDataService, IOnInit
    {
        private readonly IPoeTradeClient poeApiClient;

        public StatDataService(IPoeTradeClient poeApiClient)
        {
            this.poeApiClient = poeApiClient;
        }

        public List<StatDataCategory> Categories { get; private set; }

        private List<(Regex Regex, StatData Data)> Patterns { get; set; }

        public async Task OnInit()
        {
            Categories = null;
            Categories = await poeApiClient.Fetch<StatDataCategory>();

            Patterns = Categories
                .SelectMany(x => x.Entries)
                .Select(x => (
                    new Regex(Regex.Escape(x.Text).Replace("\\#", "([\\d,\\.]+)")),
                    x
                ))
                .ToList();
        }

        public StatData GetStat(string text)
        {
            return Patterns
                .Where(x => x.Regex.IsMatch(text))
                .Select(x => x.Data)
                .FirstOrDefault();
        }
    }
}
