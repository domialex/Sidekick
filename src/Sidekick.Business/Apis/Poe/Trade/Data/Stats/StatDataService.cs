using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Sidekick.Business.Apis.Poe.Models;
using Sidekick.Business.Apis.Poe.Trade.Data.Stats.Pseudo;
using Sidekick.Domain.Languages;

namespace Sidekick.Business.Apis.Poe.Trade.Data.Stats
{
    public class StatDataService : IStatDataService
    {
        private readonly IPseudoStatDataService pseudoStatDataService;
        private readonly ILanguageProvider languageProvider;

        public StatDataService(
            IPseudoStatDataService pseudoStatDataService,
            ILanguageProvider languageProvider)
        {
            this.pseudoStatDataService = pseudoStatDataService;
            this.languageProvider = languageProvider;
        }

        public List<StatData> PseudoPatterns { get; set; }
        public List<StatData> ExplicitPatterns { get; set; }
        public List<StatData> ImplicitPatterns { get; set; }
        public List<StatData> EnchantPatterns { get; set; }
        public List<StatData> CraftedPatterns { get; set; }
        public List<StatData> VeiledPatterns { get; set; }
        public List<StatData> FracturedPatterns { get; set; }

        public Regex NewLinePattern { get; set; } = new Regex("(?:\\\\)*[\\r\\n]+");
        public Regex IncreasedPattern { get; set; }
        public Regex AdditionalProjectilePattern { get; set; }

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
            //FillMods(mods.Veiled, VeiledPatterns, text);
            FillMods(mods.Fractured, FracturedPatterns, text);

            FillPseudo(mods.Pseudo, mods.Explicit);
            FillPseudo(mods.Pseudo, mods.Implicit);
            FillPseudo(mods.Pseudo, mods.Enchant);
            FillPseudo(mods.Pseudo, mods.Crafted);

            mods.Pseudo.ForEach(x =>
            {
                x.Text = ParseHashPattern.Replace(x.Text, ((int)x.Values[0]).ToString(), 1);
            });

            return mods;
        }

        private readonly Regex ParseHashPattern = new Regex("\\#");

        private void FillMods(List<Modifier> mods, List<StatData> patterns, string text)
        {
            var unorderedMods = new List<Modifier>();

            foreach (var data in patterns
                .AsParallel()
                .Where(x => x.Pattern != null && x.Pattern.IsMatch(text)))
            {
                FillMod(unorderedMods, text, data, data.Pattern.Match(text));
            }

            foreach (var data in patterns
                .AsParallel()
                .Where(x => x.NegativePattern != null && x.NegativePattern.IsMatch(text)))
            {
                FillMod(unorderedMods, text, data, data.NegativePattern.Match(text), true);
            }

            foreach (var data in patterns
                .AsParallel()
                .Where(x => x.AdditionalProjectilePattern != null && x.AdditionalProjectilePattern.IsMatch(text)))
            {
                FillMod(unorderedMods, text, data, data.AdditionalProjectilePattern.Match(text));
            }

            unorderedMods.OrderBy(x => x.Index).ToList().ForEach(x => mods.Add(x));
        }

        private void FillMod(List<Modifier> mods, string text, StatData data, Match result, bool negative = false)
        {
            var modifier = new Modifier()
            {
                Index = result.Index,
                Id = data.Id,
                Text = data.Text,
                Category = !data.Id.StartsWith("explicit") ? data.Category : null,
            };

            if (result.Groups.Count > 1)
            {
                for (var index = 1; index < result.Groups.Count; index++)
                {
                    if (double.TryParse(result.Groups[index].Value, out var parsedValue))
                    {
                        var modifierText = modifier.Text;
                        if (negative)
                        {
                            parsedValue *= -1;
                            modifierText = IncreasedPattern.Replace(modifierText, languageProvider.Language.ModifierReduced);
                        }

                        modifier.Values.Add(parsedValue);
                        modifier.Text = ParseHashPattern.Replace(modifierText, result.Groups[index].Value, 1);
                    }
                }

                if (data.Option?.Options?.Any() == true)
                {
                    var optionMod = data.Option.Options.SingleOrDefault(x => x.Pattern.IsMatch(text));
                    if (optionMod == null)
                    {
                        return;
                    }
                    modifier.OptionValue = optionMod;
                    modifier.Text = ParseHashPattern.Replace(modifier.Text, optionMod.Text, 1);
                }
            }

            modifier.Text = modifier.Text.Replace("+-", "-");

            mods.Add(modifier);
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

            result = FracturedPatterns.FirstOrDefault(x => x.Id == id);
            if (result != null)
            {
                return result;
            }

            return null;
        }
    }
}
