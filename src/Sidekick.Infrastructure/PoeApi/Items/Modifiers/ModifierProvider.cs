using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Sidekick.Domain.Cache;
using Sidekick.Domain.Game.Items;
using Sidekick.Domain.Game.Items.Modifiers;
using Sidekick.Domain.Game.Modifiers;
using Sidekick.Domain.Game.Modifiers.Models;
using Sidekick.Infrastructure.PoeApi.Items.AlternateModifiers;
using Sidekick.Infrastructure.PoeApi.Items.Modifiers.Models;

namespace Sidekick.Infrastructure.PoeApi.Items.Modifiers
{
    public class ModifierProvider : IModifierProvider
    {
        private readonly IPseudoModifierProvider pseudoModifierProvider;
        private readonly ICacheRepository cacheRepository;
        private readonly IPoeTradeClient poeTradeClient;
        private readonly IAlternateModifierProvider alternateModifierProvider;

        private readonly Regex ParseHashPattern = new Regex("\\#");

        public ModifierProvider(
            IPseudoModifierProvider pseudoModifierProvider,
            ICacheRepository cacheRepository,
            IPoeTradeClient poeTradeClient,
            IAlternateModifierProvider alternateModifierProvider)
        {
            this.pseudoModifierProvider = pseudoModifierProvider;
            this.cacheRepository = cacheRepository;
            this.poeTradeClient = poeTradeClient;
            this.alternateModifierProvider = alternateModifierProvider;
        }

        public List<ModifierPatternMetadata> PseudoPatterns { get; set; }
        public List<ModifierPatternMetadata> ExplicitPatterns { get; set; }
        public List<ModifierPatternMetadata> ImplicitPatterns { get; set; }
        public List<ModifierPatternMetadata> EnchantPatterns { get; set; }
        public List<ModifierPatternMetadata> CraftedPatterns { get; set; }
        public List<ModifierPatternMetadata> VeiledPatterns { get; set; }
        public List<ModifierPatternMetadata> FracturedPatterns { get; set; }

        private Regex NewLinePattern { get; set; } = new Regex("(?:\\\\)*[\\r\\n]+");

        public async Task Initialize()
        {
            await alternateModifierProvider.Initialize();

            var result = await cacheRepository.GetOrSet(
                "Sidekick.Infrastructure.PoeApi.Items.Modifiers.ModifierProvider.Initialize",
                () => poeTradeClient.Fetch<ApiCategory>("data/stats"));
            var categories = result.Result;

            PseudoPatterns = new List<ModifierPatternMetadata>();
            ExplicitPatterns = new List<ModifierPatternMetadata>();
            ImplicitPatterns = new List<ModifierPatternMetadata>();
            EnchantPatterns = new List<ModifierPatternMetadata>();
            CraftedPatterns = new List<ModifierPatternMetadata>();
            VeiledPatterns = new List<ModifierPatternMetadata>();
            FracturedPatterns = new List<ModifierPatternMetadata>();

            foreach (var category in categories)
            {
                if (!category.Entries.Any())
                {
                    continue;
                }

                // The notes in parentheses are never translated by the game.
                // We should be fine hardcoding them this way.
                List<ModifierPatternMetadata> patterns;
                var categoryLabel = category.Entries[0].Id.Split('.').First();
                switch (categoryLabel)
                {
                    default: continue;
                    case "pseudo": patterns = PseudoPatterns; break;
                    case "delve":
                    case "monster":
                    case "explicit": patterns = ExplicitPatterns; break;
                    case "implicit": patterns = ImplicitPatterns; break;
                    case "enchant": patterns = EnchantPatterns; break;
                    case "crafted": patterns = CraftedPatterns; break;
                    case "veiled": patterns = VeiledPatterns; break;
                    case "fractured": patterns = FracturedPatterns; break;
                }

                foreach (var entry in category.Entries)
                {
                    var modifier = new ModifierPatternMetadata()
                    {
                        Category = categoryLabel,
                        Id = entry.Id,
                        Patterns = new List<ModifierPattern>()
                        {
                            new ModifierPattern()
                            {
                                Text = entry.Text,
                            },
                        },
                    };

                    if (entry.Option?.Options?.Any() ?? false)
                    {
                        modifier.Options = new List<ModifierOptionParse>();

                        foreach (var entryOption in entry.Option.Options)
                        {
                            modifier.Options.Add(new ModifierOptionParse()
                            {
                                Text = entryOption.Text,
                            });
                        }
                    }

                    modifier.Patterns.Add(new ModifierPattern()
                    {
                        Text = entry.Text,
                    });

                    patterns.Add(modifier);
                }
            }

            ComputePatterns(PseudoPatterns);
            ComputePatterns(ExplicitPatterns);
            ComputePatterns(ImplicitPatterns);
            ComputePatterns(EnchantPatterns);
            ComputePatterns(CraftedPatterns);
            ComputePatterns(VeiledPatterns);
            ComputePatterns(FracturedPatterns);
        }

        private void ComputePatterns(List<ModifierPatternMetadata> metadatas)
        {
            var hashPattern = new Regex("\\\\#");
            var parenthesesPattern = new Regex("((?:\\\\\\ )*\\\\\\([^\\(\\)]*\\\\\\))");

            foreach (var metadata in metadatas)
            {
                // The notes in parentheses are never translated by the game.
                // We should be fine hardcoding them this way.
                var suffix = metadata.Category switch
                {
                    "implicit" => "\\ \\(implicit\\)",
                    "enchant" => "\\ \\(enchant\\)",
                    "crafted" => "\\ \\(crafted\\)",
                    "veiled" => "\\ \\(veiled\\)",
                    "fractured" => "\\ \\(fractured\\)",
                    _ => "",
                };

                foreach (var pattern in metadata.Patterns)
                {
                    var patternValue = Regex.Escape(pattern.Text);
                    patternValue = parenthesesPattern.Replace(patternValue, "(?:$1)?");
                    patternValue = NewLinePattern.Replace(patternValue, "\\n");
                    patternValue = hashPattern.Replace(patternValue, "(.*)");

                    pattern.Pattern = new Regex($"^{patternValue}{suffix}$", RegexOptions.None);
                }

                if (metadata.Options != null)
                {
                    foreach (var option in metadata.Options)
                    {
                        var patternValue = Regex.Escape(option.Text);
                        patternValue = NewLinePattern.Replace(patternValue, "\\n");

                        option.Pattern = new Regex(patternValue, RegexOptions.None);
                    }
                }
            }
        }

        public ItemModifiers Parse(ParsingItem parsingItem)
        {
            var mods = new ItemModifiers();

            ParseModifiers(mods.Explicit, ExplicitPatterns, parsingItem);
            ParseModifiers(mods.Enchant, EnchantPatterns, parsingItem);
            ParseModifiers(mods.Implicit, ImplicitPatterns, parsingItem);
            ParseModifiers(mods.Crafted, CraftedPatterns, parsingItem);
            ParseModifiers(mods.Fractured, FracturedPatterns, parsingItem);
            // ParseModifiers(mods.Veiled, VeiledPatterns, parsingItem);

            mods.Pseudo = pseudoModifierProvider.Parse(mods);

            return mods;
        }

        private void ParseModifiers(List<Modifier> modifiers, List<ModifierPatternMetadata> patterns, ParsingItem item)
        {
            foreach (var block in item.Blocks.Where(x => !x.Parsed))
            {
                if (ParseModifierBlock(modifiers, patterns, block))
                {
                    return;
                }
            }
        }

        private bool ParseModifierBlock(List<Modifier> modifiers, List<ModifierPatternMetadata> patterns, ParsingBlock block)
        {
            var found = false;

            foreach (var line in block.Lines.Where(x => !x.Parsed))
            {
                if (ParseModifierLine(modifiers, patterns, line))
                {
                    found = true;
                }
            }

            return found;
        }

        private bool ParseModifierLine(List<Modifier> modifiers, List<ModifierPatternMetadata> metadatas, ParsingLine line)
        {
            foreach (var metadata in metadatas)
            {
                foreach (var pattern in metadata.Patterns)
                {
                    if (pattern.Pattern.IsMatch(line.Text))
                    {
                        ParseMod(modifiers, metadata, pattern, line.Text);
                        line.Parsed = true;
                        return true;
                    }
                }
            }

            return false;
        }

        private void ParseMod(List<Modifier> modifiers, ModifierPatternMetadata data, ModifierPattern pattern, string text)
        {
            var match = pattern.Pattern.Match(text);

            var modifier = new Modifier()
            {
                Index = match.Index,
                Id = data.Id,
                Text = pattern.Text,
                Category = !data.Id.StartsWith("explicit") ? data.Category : null,
            };

            if (match.Groups.Count > 1)
            {
                for (var index = 1; index < match.Groups.Count; index++)
                {
                    if (double.TryParse(match.Groups[index].Value, NumberStyles.Any, CultureInfo.InvariantCulture, out var parsedValue))
                    {
                        if (pattern.Negated)
                        {
                            parsedValue *= -1;
                        }

                        modifier.Values.Add(parsedValue);
                        modifier.Text = ParseHashPattern.Replace(modifier.Text, match.Groups[index].Value, 1);
                    }
                }

                if (data.Options?.Any() ?? false)
                {
                    var optionMod = data.Options.SingleOrDefault(x => x.Pattern.IsMatch(text));
                    if (optionMod != null)
                    {
                        modifier.OptionValue = new ModifierOption()
                        {
                            Text = optionMod.Text,
                        };
                        modifier.Text = ParseHashPattern.Replace(modifier.Text, optionMod.Text, 1);
                    }
                }
            }

            modifier.Text = modifier.Text.Replace("+-", "-");

            modifiers.Add(modifier);
        }
        /*
        private void ParseAlternateMod(List<Modifier> mods, string text, AlternateModifier alternateModifier, ModifierPattern data, Match result)
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
                var negate = condition.Negated ?? false;

                if (condition.Min != null && condition.Max == null)
                    modifier.Values.Add(condition.Min.Value * (negate ? -1 : 1));
                else if (condition.Min == null && condition.Max != null)
                    modifier.Values.Add(condition.Max.Value * (negate ? -1 : 1));
                else if (condition.Min != null && condition.Max != null && condition.Min == condition.Max)
                    modifier.Values.Add(condition.Min.Value * (negate ? -1 : 1));
            }
            if (data.Options?.Any())
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
        */
        public bool IsMatch(string id, string text)
        {
            ModifierPatternMetadata metadata = null;

            metadata = PseudoPatterns.FirstOrDefault(x => x.Id == id);
            if (metadata == null) metadata = ImplicitPatterns.FirstOrDefault(x => x.Id == id);
            if (metadata == null) metadata = ExplicitPatterns.FirstOrDefault(x => x.Id == id);
            if (metadata == null) metadata = CraftedPatterns.FirstOrDefault(x => x.Id == id);
            if (metadata == null) metadata = EnchantPatterns.FirstOrDefault(x => x.Id == id);
            if (metadata == null) metadata = FracturedPatterns.FirstOrDefault(x => x.Id == id);


            if (metadata == null)
            {
                return false;
            }

            foreach (var pattern in metadata.Patterns)
            {
                if (pattern.Pattern != null && pattern.Pattern.IsMatch(text))
                {
                    return true;
                }
            }

            return false;
        }
    }
}
