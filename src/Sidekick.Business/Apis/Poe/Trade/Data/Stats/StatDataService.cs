using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Sidekick.Business.Apis.Poe.Models;
using Sidekick.Business.Apis.Poe.Trade.Data.Stats.Pseudo;
using Sidekick.Core.Initialization;

namespace Sidekick.Business.Apis.Poe.Trade.Data.Stats
{
    public class StatDataService : IStatDataService, IOnInit
    {
        private readonly IPoeTradeClient poeApiClient;
        private readonly IPseudoStatDataService pseudoStatDataService;

        public StatDataService(IPoeTradeClient poeApiClient,
            IPseudoStatDataService pseudoStatDataService)
        {
            this.poeApiClient = poeApiClient;
            this.pseudoStatDataService = pseudoStatDataService;
        }

        private List<StatData> PseudoPatterns { get; set; }

        private List<StatData> ExplicitPatterns { get; set; }

        private List<StatData> ImplicitPatterns { get; set; }

        private List<StatData> EnchantPatterns { get; set; }

        private List<StatData> CraftedPatterns { get; set; }

        private List<StatData> VeiledPatterns { get; set; }

        private Regex NewLinePattern { get; set; }

        public async Task OnInit()
        {
            var categories = await poeApiClient.Fetch<StatDataCategory>();

            PseudoPatterns = new List<StatData>();
            ExplicitPatterns = new List<StatData>();
            ImplicitPatterns = new List<StatData>();
            EnchantPatterns = new List<StatData>();
            CraftedPatterns = new List<StatData>();
            VeiledPatterns = new List<StatData>();

            NewLinePattern = new Regex("(?:\\\\)*[\\r\\n]+");

            var hashPattern = new Regex("\\\\#");
            var parenthesesPattern = new Regex("((?:\\\\\\ )*\\\\\\([^\\(\\)]*\\\\\\))");

            foreach (var category in categories)
            {
                var first = category.Entries.FirstOrDefault();
                if (first == null)
                {
                    continue;
                }

                // The notes in parentheses are never translated by the game.
                // We should be fine hardcoding them this way.
                string suffix, pattern;
                List<StatData> patterns;
                switch (first.Id.Split('.').First())
                {
                    default: continue;
                    case "pseudo": suffix = "\\ *\\n+"; patterns = PseudoPatterns; break;
                    case "delve":
                    case "monster":
                    case "explicit": suffix = "\\ *\\n+"; patterns = ExplicitPatterns; break;
                    case "implicit": suffix = "\\ *\\(implicit\\)"; patterns = ImplicitPatterns; break;
                    case "enchant": suffix = "\\ *\\(enchant\\)"; patterns = EnchantPatterns; break;
                    case "crafted": suffix = "\\ *\\(crafted\\)"; patterns = CraftedPatterns; break;
                    case "veiled": suffix = "\\ *\\(veiled\\)"; patterns = VeiledPatterns; break;
                }

                foreach (var entry in category.Entries)
                {
                    entry.Category = category.Label;

                    pattern = Regex.Escape(entry.Text);
                    pattern = parenthesesPattern.Replace(pattern, "(?:$1)?");
                    pattern = hashPattern.Replace(pattern, "([-+\\d,\\.]+)");
                    pattern = NewLinePattern.Replace(pattern, "\\n");

                    entry.Pattern = new Regex($"\\n+{pattern}{suffix}");
                    patterns.Add(entry);
                }
            }
        }

        public Modifiers ParseMods(string text)
        {
            text = NewLinePattern.Replace(text, "\n");

            var mods = new Modifiers();

            // Make sure the text ends with an empty line for our regexes to work correctly
            if (!text.EndsWith("\n"))
            {
                text += "\n";
            }

            FillMods(mods.Explicit, ExplicitPatterns, text);
            FillMods(mods.Implicit, ImplicitPatterns, text);
            FillMods(mods.Enchant, EnchantPatterns, text);
            FillMods(mods.Crafted, CraftedPatterns, text);
            // FillMods(mods.Veiled, VeiledPatterns, text);

            FillPseudo(mods.Pseudo, mods.Explicit);
            FillPseudo(mods.Pseudo, mods.Implicit);
            FillPseudo(mods.Pseudo, mods.Enchant);
            FillPseudo(mods.Pseudo, mods.Crafted);

            return mods;
        }

        private void FillMods(List<Modifier> mods, List<StatData> patterns, string text)
        {
            foreach (var x in patterns
                .Where(x => x.Pattern.IsMatch(text)))
            {
                var result = x.Pattern.Match(text);
                var modifier = new Modifier()
                {
                    Id = x.Id,
                    Text = x.Text,
                };

                if (result.Groups.Count > 1)
                {
                    for (var index = 1; index < result.Groups.Count; index++)
                    {
                        if (double.TryParse(result.Groups[index].Value, out var parsedValue))
                        {
                            modifier.Values.Add(parsedValue);
                        }
                    }
                }

                mods.Add(modifier);
            }
        }

        private void FillPseudo(List<Modifier> pseudoMods, List<Modifier> mods)
        {
            Modifier pseudoMod;
            Modifier mod;
            foreach (var pseudoDefinition in pseudoStatDataService.Definitions)
            {
                foreach (var pseudoModifier in pseudoDefinition.Modifiers)
                {
                    mod = mods.FirstOrDefault(x => pseudoModifier.Ids.Any(id => id == x.Id));
                    if (mod != null)
                    {
                        pseudoMod = pseudoMods.FirstOrDefault(x => x.Id == pseudoDefinition.Id);
                        if (pseudoMod == null)
                        {
                            pseudoMod = new Modifier()
                            {
                                Id = pseudoDefinition.Id,
                                Text = pseudoDefinition.Text,
                            };
                            pseudoMod.Values.Add((int)(mod.Values.FirstOrDefault() * pseudoModifier.Multiplier));
                            pseudoMods.Add(pseudoMod);
                        }
                        else
                        {
                            pseudoMod.Values[0] += (int)(mod.Values.FirstOrDefault() * pseudoModifier.Multiplier);
                        }
                    }
                }
            }
        }

        public StatData GetById(string id)
        {
            var result = PseudoPatterns.FirstOrDefault(x => x.Id == id);
            if (result != null)
            {
                return result;
            }

            result = ImplicitPatterns.FirstOrDefault(x => x.Id == id);
            if (result != null)
            {
                return result;
            }

            result = ExplicitPatterns.FirstOrDefault(x => x.Id == id);
            if (result != null)
            {
                return result;
            }

            result = CraftedPatterns.FirstOrDefault(x => x.Id == id);
            if (result != null)
            {
                return result;
            }

            result = EnchantPatterns.FirstOrDefault(x => x.Id == id);
            if (result != null)
            {
                return result;
            }

            return null;
        }
    }
}
