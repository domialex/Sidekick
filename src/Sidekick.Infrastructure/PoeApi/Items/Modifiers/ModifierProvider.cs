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
using Sidekick.Infrastructure.PoeApi.Items.Modifiers.Models;
using Sidekick.Infrastructure.RePoe.Data.StatTranslations;

namespace Sidekick.Infrastructure.PoeApi.Items.Modifiers
{
    public class ModifierProvider : IModifierProvider
    {
        private readonly IPseudoModifierProvider pseudoModifierProvider;
        private readonly ICacheRepository cacheRepository;
        private readonly IPoeTradeClient poeTradeClient;
        private readonly IStatTranslationProvider statTranslationProvider;

        private readonly Regex ParseHashPattern = new Regex("\\#");
        private readonly Regex NewLinePattern = new Regex("(?:\\\\)*[\\r\\n]+");
        private readonly Regex hashPattern = new Regex("\\\\#");
        private readonly Regex parenthesesPattern = new Regex("((?:\\\\\\ )*\\\\\\([^\\(\\)]*\\\\\\))");

        public ModifierProvider(
            IPseudoModifierProvider pseudoModifierProvider,
            ICacheRepository cacheRepository,
            IPoeTradeClient poeTradeClient,
            IStatTranslationProvider statTranslationProvider)
        {
            this.pseudoModifierProvider = pseudoModifierProvider;
            this.cacheRepository = cacheRepository;
            this.poeTradeClient = poeTradeClient;
            this.statTranslationProvider = statTranslationProvider;
        }

        public List<ModifierPatternMetadata> PseudoPatterns { get; set; }
        public List<ModifierPatternMetadata> ExplicitPatterns { get; set; }
        public List<ModifierPatternMetadata> ImplicitPatterns { get; set; }
        public List<ModifierPatternMetadata> EnchantPatterns { get; set; }
        public List<ModifierPatternMetadata> CraftedPatterns { get; set; }
        public List<ModifierPatternMetadata> VeiledPatterns { get; set; }
        public List<ModifierPatternMetadata> FracturedPatterns { get; set; }

        public async Task Initialize()
        {
            await statTranslationProvider.Initialize();

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
                        IsOption = entry.Option?.Options?.Any() ?? false,
                    };

                    if (modifier.IsOption)
                    {
                        for (var i = 0; i < entry.Option.Options.Count; i++)
                        {
                            modifier.Patterns.Add(new ModifierPattern()
                            {
                                Text = ComputeOptionText(entry.Text, entry.Option.Options[i].Text),
                                LineCount = NewLinePattern.Matches(entry.Text).Count + NewLinePattern.Matches(entry.Option.Options[i].Text).Count + 1,
                                Value = i + 1,
                                Pattern = ComputePattern(categoryLabel, entry.Text, entry.Option.Options[i].Text),
                            });
                        }
                    }
                    else
                    {
                        var stats = statTranslationProvider.GetAlternateModifiers(entry.Text);

                        if (stats != null)
                        {
                            foreach(var stat in stats)
                            {
                                modifier.Patterns.Add(new ModifierPattern()
                                {
                                    Text = stat.Text,
                                    LineCount = NewLinePattern.Matches(stat.Text).Count + 1,
                                    Pattern = ComputePattern(categoryLabel, stat.Text),
                                    Negative = stat.Negative,
                                    Value = stat.Value,
                                });
                            }
                        }
                        else
                        {
                            modifier.Patterns.Add(new ModifierPattern()
                            {
                                Text = entry.Text,
                                LineCount = NewLinePattern.Matches(entry.Text).Count + 1,
                                Pattern = ComputePattern(categoryLabel, entry.Text),
                            });
                        }
                    }

                    patterns.Add(modifier);
                }
            }
        }

        private Regex ComputePattern(string category, string text, string optionText = null)
        {
            // The notes in parentheses are never translated by the game.
            // We should be fine hardcoding them this way.
            var suffix = category switch
            {
                "implicit" => "(?:\\ \\(implicit\\))?",
                "enchant" => "(?:\\ \\(enchant\\))?",
                "crafted" => "(?:\\ \\(crafted\\))?",
                "veiled" => "(?:\\ \\(veiled\\))?",
                "fractured" => "(?:\\ \\(fractured\\))?",
                _ => "",
            };

            var patternValue = Regex.Escape(text);
            patternValue = parenthesesPattern.Replace(patternValue, "(?:$1)?");
            patternValue = NewLinePattern.Replace(patternValue, "\\n");

            if (string.IsNullOrEmpty(optionText))
            {
                patternValue = hashPattern.Replace(patternValue, "(.*)");
            }
            else
            {
                var optionLines = new List<string>();
                foreach (var optionLine in NewLinePattern.Split(optionText))
                {
                    optionLines.Add(hashPattern.Replace(patternValue, Regex.Escape(optionLine)));
                }
                patternValue = string.Join('\n', optionLines);
            }

            return new Regex($"^{patternValue}{suffix}$", RegexOptions.None);
        }

        private string ComputeOptionText(string text, string optionText)
        {
            var optionLines = new List<string>();
            foreach (var optionLine in NewLinePattern.Split(optionText))
            {
                optionLines.Add(ParseHashPattern.Replace(text, optionLine));
            }
            return string.Join('\n', optionLines);
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
                if (ParseModifierLine(modifiers, patterns, block, line))
                {
                    found = true;
                }
            }

            return found;
        }

        private bool ParseModifierLine(List<Modifier> modifiers, List<ModifierPatternMetadata> metadatas, ParsingBlock block, ParsingLine line)
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

                    // Multiline modifiers
                    else if (pattern.LineCount > 1 && pattern.Pattern.IsMatch(string.Join('\n', block.Lines.Skip(line.Index).Take(pattern.LineCount))))
                    {
                        ParseMod(modifiers, metadata, pattern, string.Join('\n', block.Lines.Skip(line.Index).Take(pattern.LineCount)));
                        foreach (var multiline in block.Lines.Skip(line.Index).Take(pattern.LineCount))
                        {
                            multiline.Parsed = true;
                        }
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

            if (data.IsOption)
            {
                modifier.OptionValue = new ModifierOption()
                {
                    Value = pattern.Value,
                };
            }
            else if (pattern.Value.HasValue)
            {
                modifier.Values.Add(pattern.Value.Value);
                modifier.Text = ParseHashPattern.Replace(modifier.Text, match.Groups[1].Value, 1);
            }
            else if (match.Groups.Count > 1)
            {
                for (var index = 1; index < match.Groups.Count; index++)
                {
                    if (double.TryParse(match.Groups[index].Value, NumberStyles.Any, CultureInfo.InvariantCulture, out var parsedValue))
                    {
                        if (pattern.Negative)
                        {
                            parsedValue *= -1;
                        }

                        modifier.Values.Add(parsedValue);
                        modifier.Text = ParseHashPattern.Replace(modifier.Text, match.Groups[index].Value, 1);
                    }
                }

                modifier.Text = modifier.Text.Replace("+-", "-");
            }

            modifiers.Add(modifier);
        }

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
