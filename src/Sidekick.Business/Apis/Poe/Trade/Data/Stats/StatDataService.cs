using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Sidekick.Business.Apis.Poe.Models;
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

        private List<(Regex Regex, StatData Data)> ExplicitPatterns { get; set; }

        private List<(Regex Regex, StatData Data)> ImplicitPatterns { get; set; }

        private List<(Regex Regex, StatData Data)> EnchantPatterns { get; set; }

        private List<(Regex Regex, StatData Data)> CraftedPatterns { get; set; }

        private List<(Regex Regex, StatData Data)> VeiledPatterns { get; set; }

        public async Task OnInit()
        {
            Categories = null;
            Categories = await poeApiClient.Fetch<StatDataCategory>();

            ExplicitPatterns = new List<(Regex Regex, StatData Data)>();
            ImplicitPatterns = new List<(Regex Regex, StatData Data)>();
            EnchantPatterns = new List<(Regex Regex, StatData Data)>();
            CraftedPatterns = new List<(Regex Regex, StatData Data)>();
            VeiledPatterns = new List<(Regex Regex, StatData Data)>();

            foreach (var category in Categories)
            {
                var first = category.Entries.FirstOrDefault();
                if (first == null)
                {
                    continue;
                }

                // The notes in parentheses are never translated by the game.
                // We should be fine hardcoding them this way.
                string suffix;
                List<(Regex Regex, StatData Data)> patterns;
                switch (first.Id.Split('.').First())
                {
                    default: continue;
                    case "delve":
                    case "monster":
                    case "explicit": suffix = "\\ *[\\r\\n]+"; patterns = ExplicitPatterns; break;
                    case "implicit": suffix = "\\ *\\(implicit\\)"; patterns = ImplicitPatterns; break;
                    case "enchant": suffix = "\\ *\\(enchant\\)"; patterns = EnchantPatterns; break;
                    case "crafted": suffix = "\\ *\\(crafted\\)"; patterns = CraftedPatterns; break;
                    case "veiled": suffix = "\\ *\\(veiled\\)"; patterns = VeiledPatterns; break;
                }

                foreach (var entry in category.Entries)
                {
                    patterns.Add((new Regex($"[\\r\\n]+{Regex.Escape(entry.Text).Replace("\\#", "([-+\\d,\\.]+)")}{suffix}"), entry));
                }
            }

            var test = ExplicitPatterns.Where(x => x.Data.Id.Contains("stat_3299347043")).FirstOrDefault();
        }

        public Mods GetMods(string text)
        {
            var mods = new Mods();

            // Make sure the text ends with an empty line for our regexes to work correctly
            if (!text.EndsWith("\\r\\n"))
            {
                text += "\\r\\n";
            }

            FillMods(mods.Explicit, ExplicitPatterns, text);
            FillMods(mods.Implicit, ImplicitPatterns, text);
            FillMods(mods.Enchant, EnchantPatterns, text);
            FillMods(mods.Crafted, CraftedPatterns, text);
            // FillMods(mods.Veiled, VeiledPatterns, text);

            return mods;
        }

        private void FillMods(List<Mod> mods, List<(Regex Regex, StatData Data)> patterns, string text)
        {
            var results = patterns
                .Where(x => x.Regex.IsMatch(text))
                .ToList();

            foreach (var x in results)
            {
                var result = x.Regex.Match(text);
                var magnitudes = new List<Magnitude>();

                for (var index = 1; index < result.Groups.Count; index++)
                {
                    magnitudes.Add(new Magnitude()
                    {
                        Hash = x.Data.Id,
                        Max = null,
                        Min = null,
                    });
                }

                mods.Add(new Mod()
                {
                    Magnitudes = magnitudes
                });
            }
        }
    }
}
