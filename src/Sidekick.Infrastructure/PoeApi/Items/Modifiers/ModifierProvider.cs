using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Sidekick.Domain.Cache;
using Sidekick.Domain.Game.Items;
using Sidekick.Domain.Game.Items.Modifiers;
using Sidekick.Domain.Game.Languages;
using Sidekick.Domain.Game.Modifiers;
using Sidekick.Domain.Game.Modifiers.Models;
using Sidekick.Domain.Game.StatTranslations;
using Sidekick.Infrastructure.PoeApi.Items.Modifiers.Models;

namespace Sidekick.Infrastructure.PoeApi.Items.Modifiers
{
    public class ModifierProvider : IModifierProvider
    {
        private readonly IPseudoModifierProvider pseudoModifierProvider;
        private readonly IGameLanguageProvider gameLanguageProvider;
        private readonly ICacheRepository cacheRepository;
        private readonly IPoeTradeClient poeTradeClient;
        private readonly IAlternateStatProvider statTranslationProvider;

        private readonly Regex ParseHashPattern = new Regex("\\#");

        public ModifierProvider(
            IPseudoModifierProvider pseudoModifierProvider,
            IGameLanguageProvider gameLanguageProvider,
            ICacheRepository cacheRepository,
            IPoeTradeClient poeTradeClient,
            IAlternateStatProvider statTranslationProvider)
        {
            this.pseudoModifierProvider = pseudoModifierProvider;
            this.gameLanguageProvider = gameLanguageProvider;
            this.cacheRepository = cacheRepository;
            this.poeTradeClient = poeTradeClient;
            this.statTranslationProvider = statTranslationProvider;
        }

        public List<ModifierPattern> PseudoPatterns { get; set; }
        public List<ModifierPattern> ExplicitPatterns { get; set; }
        public List<ModifierPattern> ImplicitPatterns { get; set; }
        public List<ModifierPattern> EnchantPatterns { get; set; }
        public List<ModifierPattern> CraftedPatterns { get; set; }
        public List<ModifierPattern> VeiledPatterns { get; set; }
        public List<ModifierPattern> FracturedPatterns { get; set; }

        public Regex NewLinePattern { get; set; } = new Regex("(?:\\\\)*[\\r\\n]+");
        public Regex IncreasedPattern { get; set; }
        public Regex AdditionalProjectilePattern { get; set; }

        public async Task Initialize()
        {
            var result = await cacheRepository.GetOrSet(
                "Sidekick.Infrastructure.PoeApi.Items.Modifiers.ModifierProvider.Initialize",
                () => poeTradeClient.Fetch<ApiCategory>("data/stats"));
            var categories = result.Result;

            PseudoPatterns = new List<ModifierPattern>();
            ExplicitPatterns = new List<ModifierPattern>();
            ImplicitPatterns = new List<ModifierPattern>();
            EnchantPatterns = new List<ModifierPattern>();
            CraftedPatterns = new List<ModifierPattern>();
            VeiledPatterns = new List<ModifierPattern>();
            FracturedPatterns = new List<ModifierPattern>();
            IncreasedPattern = new Regex(gameLanguageProvider.Language.ModifierIncreased);

            var hashPattern = new Regex("\\\\#");
            var parenthesesPattern = new Regex("((?:\\\\\\ )*\\\\\\([^\\(\\)]*\\\\\\))");

            var additionalProjectileEscaped = Regex.Escape(gameLanguageProvider.Language.AdditionalProjectile);
            var additionalProjectiles = hashPattern.Replace(Regex.Escape(gameLanguageProvider.Language.AdditionalProjectiles), "([-+\\d,\\.]+)");

            // We need to ignore the case here, there are some mistakes in the data of the game.
            AdditionalProjectilePattern = new Regex(gameLanguageProvider.Language.AdditionalProjectile, RegexOptions.IgnoreCase);

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
                List<ModifierPattern> patterns;
                switch (first.Id.Split('.').First())
                {
                    default: continue;
                    case "pseudo": suffix = "(?:\\n|(?<!(?:\\n.*){2,})$)"; patterns = PseudoPatterns; break;
                    case "delve":
                    case "monster":
                    case "explicit": suffix = "(?:\\n|(?<!(?:\\n.*){2,})$)"; patterns = ExplicitPatterns; break;
                    case "implicit": suffix = "(?:\\ \\(implicit\\)\\n|(?<!(?:\\n.*){2,})$)"; patterns = ImplicitPatterns; break;
                    case "enchant": suffix = "(?:\\ \\(enchant\\)\\n|(?<!(?:\\n.*){2,})$)"; patterns = EnchantPatterns; break;
                    case "crafted": suffix = "(?:\\ \\(crafted\\)\\n|(?<!(?:\\n.*){2,})$)"; patterns = CraftedPatterns; break;
                    case "veiled": suffix = "(?:\\ \\(veiled\\)\\n|(?<!(?:\\n.*){2,})$)"; patterns = VeiledPatterns; break;
                    case "fractured": suffix = "(?:\\ \\(fractured\\)\\n|(?<!(?:\\n.*){2,})$)"; patterns = FracturedPatterns; break;
                }

                foreach (var entry in category.Entries)
                {
                    var modifier = new ModifierPattern()
                    {
                        Metadata = new ModifierMetadata()
                        {
                            Category = category.Label,
                            Id = entry.Id,
                            Text = entry.Text,
                            Type = entry.Type,
                        },
                        AlternateModifiers = new List<AlternateModifier>()
                    };

                    pattern = Regex.Escape(entry.Text);
                    pattern = parenthesesPattern.Replace(pattern, "(?:$1)?");
                    pattern = NewLinePattern.Replace(pattern, "\\n");

                    if (entry.Option == null || entry.Option.Options == null || entry.Option.Options.Count == 0)
                    {
                        pattern = hashPattern.Replace(pattern, "([-+\\d,\\.]+)");
                    }
                    else
                    {
                        modifier.Options = new List<ModifierOptionParse>();

                        foreach (var entryOption in entry.Option.Options)
                        {
                            var modifierOption = new ModifierOptionParse()
                            {
                                Text = entryOption.Text,
                            };

                            if (NewLinePattern.IsMatch(entryOption.Text))
                            {
                                var lines = NewLinePattern.Split(entryOption.Text).ToList();
                                var options = lines.ConvertAll(x => hashPattern.Replace(pattern, Regex.Escape(x)));
                                modifierOption.Pattern = new Regex($"(?:^|\\n){string.Join("\\n", options)}{suffix}");
                                modifierOption.Text = string.Join("\n", lines.Select((x, i) => new
                                {
                                    Text = x,
                                    Index = i
                                })
                                .ToList()
                                .ConvertAll(x =>
                                {
                                    if (x.Index == 0)
                                    {
                                        return x.Text;
                                    }

                                    return entry.Text.Replace("#", x.Text);
                                }));
                            }
                            else
                            {
                                modifierOption.Pattern = new Regex($"(?:^|\\n){hashPattern.Replace(pattern, Regex.Escape(entryOption.Text))}{suffix}", RegexOptions.None);
                            }
                        }

                        pattern = hashPattern.Replace(pattern, "(.*)");
                    }

                    if (IncreasedPattern.IsMatch(pattern))
                    {
                        var negativePattern = IncreasedPattern.Replace(pattern, gameLanguageProvider.Language.ModifierReduced);
                        modifier.NegativePattern = new Regex($"(?:^|\\n){negativePattern}{suffix}", RegexOptions.None);
                    }

                    if (AdditionalProjectilePattern.IsMatch(entry.Text))
                    {
                        var additionalProjectilePattern = pattern.Replace(additionalProjectileEscaped, additionalProjectiles, System.StringComparison.OrdinalIgnoreCase);
                        modifier.AdditionalProjectilePattern = new Regex($"(?:^|\\n){additionalProjectilePattern}{suffix}", RegexOptions.IgnoreCase);
                    }

                    modifier.Pattern = new Regex($"(?:^|\\n){pattern}{suffix}", RegexOptions.None);

                    var alternateStats = statTranslationProvider.GetAlternateStats(entry.Text);

                    foreach (var stat in alternateStats)
                    {
                        var altPattern = Regex.Escape(stat.Text);
                        altPattern = parenthesesPattern.Replace(altPattern, "(?:$1)?");
                        altPattern = NewLinePattern.Replace(altPattern, "\\n");
                        altPattern = hashPattern.Replace(altPattern, "([-+\\d,\\.]+)");

                        var alternateModifier = new AlternateModifier
                        {
                            Stat = stat,
                            Pattern = new Regex($"(?:^|\\n){altPattern}{suffix}", RegexOptions.None)
                        };

                        modifier.AlternateModifiers.Add(alternateModifier);
                    }

                    patterns.Add(modifier);
                }
            }
        }

        public ItemModifiers Parse(ParsingItem parsingItem)
        {
            var text = NewLinePattern.Replace(parsingItem.Text, "\n");

            var mods = new ItemModifiers();

            // Make sure the text ends with an empty line for our regexes to work correctly
            if (!text.EndsWith("\n"))
            {
                text += "\n";
            }

            ParseMods(mods.Explicit, ExplicitPatterns, text);
            ParseMods(mods.Implicit, ImplicitPatterns, text);
            ParseMods(mods.Enchant, EnchantPatterns, text);
            ParseMods(mods.Crafted, CraftedPatterns, text);
            //FillMods(mods.Veiled, VeiledPatterns, text);
            ParseMods(mods.Fractured, FracturedPatterns, text);

            mods.Pseudo = pseudoModifierProvider.Parse(mods);

            return mods;
        }

        private void ParseMods(List<Modifier> mods, List<ModifierPattern> patterns, string text)
        {
            var unorderedMods = new List<Modifier>();

            foreach (var data in patterns)
            {
                if (data.Pattern.IsMatch(text))
                {
                    ParseMod(unorderedMods, text, data, data.Pattern.Match(text));
                }
                else
                {
                    if (data.AlternateModifiers.Count > 0)
                    {
                        foreach (var alternateModifier in data.AlternateModifiers)
                        {
                            if (alternateModifier.Pattern.IsMatch(text))
                            {
                                ParseAlternateMod(unorderedMods, text, alternateModifier, data, alternateModifier.Pattern.Match(text));
                            }
                        }
                    }
                }
            }

            unorderedMods.OrderBy(x => x.Index).ToList().ForEach(x => mods.Add(x));
        }

        private void ParseAlternateMod(List<Modifier> mods, string text, AlternateModifier alternateModifier, ModifierPattern data, Match result)
        {
            var modifier = new Modifier()
            {
                Index = result.Index,
                Id = data.Metadata.Id,
                Text = data.Metadata.Text,
                Category = !data.Metadata.Id.StartsWith("explicit") ? data.Metadata.Category : null,
            };

            if (result.Groups.Count > 1)
            {
                for (var index = 1; index < result.Groups.Count; index++)
                {
                    if (double.TryParse(result.Groups[index].Value, NumberStyles.Any, CultureInfo.InvariantCulture, out var parsedValue))
                    {
                        for (int i = 0; i < alternateModifier.Stat.Conditions.Count; i++)
                        {
                            var condition = alternateModifier.Stat.Conditions[i];
                            string handler = "";

                            if (alternateModifier.Stat.IndexHandlers.Count > i + 1)
                                handler = alternateModifier.Stat.IndexHandlers[i][0];

                            var negate = handler == "negate";
                            parsedValue *= (negate ? -1 : 1);

                            if ((condition.Min != null && condition.Max != null && parsedValue >= condition.Min && parsedValue <= condition.Max) ||
                                (condition.Min != null && condition.Max == null && parsedValue >= condition.Min) ||
                                (condition.Min == null && condition.Max != null && parsedValue <= condition.Max))
                            {
                                modifier.Values.Add(parsedValue);
                                break;
                            }
                        }

                    }
                }
            }
            else if (alternateModifier.Stat.Conditions.Count == 1)
            {
                var condition = alternateModifier.Stat.Conditions[0];

                var negate = condition.Negated == null ? false : condition.Negated.Value;

                if (condition.Min != null && condition.Max == null)
                    modifier.Values.Add(condition.Min.Value * (negate ? -1 : 1));
                else if (condition.Min == null && condition.Max != null)
                    modifier.Values.Add(condition.Max.Value * (negate ? -1 : 1));
                else if (condition.Min != null && condition.Max != null && condition.Min == condition.Max)
                    modifier.Values.Add(condition.Min.Value * (negate ? -1 : 1));
            }

            if (data.Options?.Any() == true)
            {
                var optionMod = data.Options.SingleOrDefault(x => x.Pattern.IsMatch(text));
                if (optionMod == null)
                {
                    return;
                }
                modifier.OptionValue = new ModifierOption()
                {
                    Text = optionMod.Text,
                };
                modifier.Text = ParseHashPattern.Replace(modifier.Text, optionMod.Text, 1);
            }

            modifier.Text = modifier.Text.Replace("+-", "-");

            mods.Add(modifier);
        }

        private void ParseMod(List<Modifier> mods, string text, ModifierPattern data, Match result, bool negative = false)
        {
            var modifier = new Modifier()
            {
                Index = result.Index,
                Id = data.Metadata.Id,
                Text = data.Metadata.Text,
                Category = !data.Metadata.Id.StartsWith("explicit") ? data.Metadata.Category : null,
            };

            if (result.Groups.Count > 1)
            {
                for (var index = 1; index < result.Groups.Count; index++)
                {
                    if (double.TryParse(result.Groups[index].Value, NumberStyles.Any, CultureInfo.InvariantCulture, out var parsedValue))
                    {
                        var modifierText = modifier.Text;
                        if (negative)
                        {
                            parsedValue *= -1;
                            modifierText = IncreasedPattern.Replace(modifierText, gameLanguageProvider.Language.ModifierReduced);
                        }

                        modifier.Values.Add(parsedValue);
                        modifier.Text = ParseHashPattern.Replace(modifierText, result.Groups[index].Value, 1);
                    }
                }

                if (data.Options?.Any() == true)
                {
                    var optionMod = data.Options.SingleOrDefault(x => x.Pattern.IsMatch(text));
                    if (optionMod == null)
                    {
                        return;
                    }
                    modifier.OptionValue = new ModifierOption()
                    {
                        Text = optionMod.Text,
                    };
                    modifier.Text = ParseHashPattern.Replace(modifier.Text, optionMod.Text, 1);
                }
            }

            modifier.Text = modifier.Text.Replace("+-", "-");

            mods.Add(modifier);
        }

        public ModifierMetadata GetById(string id)
        {
            var result = PseudoPatterns.FirstOrDefault(x => x.Metadata.Id == id);
            if (result != null)
            {
                return result.Metadata;
            }

            result = ImplicitPatterns.FirstOrDefault(x => x.Metadata.Id == id);
            if (result != null)
            {
                return result.Metadata;
            }

            result = ExplicitPatterns.FirstOrDefault(x => x.Metadata.Id == id);
            if (result != null)
            {
                return result.Metadata;
            }

            result = CraftedPatterns.FirstOrDefault(x => x.Metadata.Id == id);
            if (result != null)
            {
                return result.Metadata;
            }

            result = EnchantPatterns.FirstOrDefault(x => x.Metadata.Id == id);
            if (result != null)
            {
                return result.Metadata;
            }

            result = FracturedPatterns.FirstOrDefault(x => x.Metadata.Id == id);
            if (result != null)
            {
                return result.Metadata;
            }

            return null;
        }

        public bool IsMatch(string id, string text)
        {
            ModifierPattern pattern = null;

            pattern = PseudoPatterns.FirstOrDefault(x => x.Metadata.Id == id);
            if (pattern == null)
            {
                pattern = ImplicitPatterns.FirstOrDefault(x => x.Metadata.Id == id);
            }
            if (pattern == null)
            {
                pattern = ExplicitPatterns.FirstOrDefault(x => x.Metadata.Id == id);
            }
            if (pattern == null)
            {
                pattern = CraftedPatterns.FirstOrDefault(x => x.Metadata.Id == id);
            }
            if (pattern == null)
            {
                pattern = EnchantPatterns.FirstOrDefault(x => x.Metadata.Id == id);
            }
            if (pattern == null)
            {
                pattern = FracturedPatterns.FirstOrDefault(x => x.Metadata.Id == id);
            }

            if (pattern == null)
            {
                return false;
            }

            if (pattern.Pattern != null && pattern.Pattern.IsMatch(text))
            {
                return true;
            }

            if (pattern.NegativePattern != null && pattern.NegativePattern.IsMatch(text))
            {
                return true;
            }

            return false;
        }
    }
}
