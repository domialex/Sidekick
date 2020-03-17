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

            Patterns = new List<(Regex Regex, StatData Data)>();
            foreach (var category in Categories)
            {
                var first = category.Entries.FirstOrDefault();
                if (first == null)
                {
                    continue;
                }

                string suffix;
                switch (first.Id.Split('.').First())
                {
                    default: continue;
                    case "delve":
                    case "monster":
                    case "explicit": suffix = "\\ *[\\r\\n]+"; break;
                    case "implicit": suffix = "\\ *\\(implicit\\)"; break;
                    case "enchant": suffix = "\\ *\\(enchant\\)"; break;
                    case "crafted": suffix = "\\ *\\(crafted\\)"; break;
                    case "veiled": suffix = "\\ *\\(veiled\\)"; break;
                }

                foreach (var entry in category.Entries)
                {
                    Patterns.Add((new Regex($"[\\r\\n]+{Regex.Escape(entry.Text).Replace("\\#", "([-+\\d,\\.]+)")}{suffix}"), entry));
                }
            }
        }

        public List<StatData> GetStats(string text)
        {
            if (!text.EndsWith("\\r\\n"))
            {
                text += "\\r\\n";
            }

            return Patterns
                .Where(x => x.Regex.IsMatch(text))
                .Select(x => x.Data)
                .ToList();
        }
    }
}
