using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Sidekick.Business.Categories;
using Sidekick.Business.Filters;
using Sidekick.Business.Languages;
using Sidekick.Business.Maps;
using Sidekick.Business.Parsers.Models;
using Sidekick.Business.Parsers.Types;
using Sidekick.Business.Tokenizers;
using Sidekick.Business.Tokenizers.ItemName;
using Sidekick.Core.Loggers;
using Attribute = Sidekick.Business.Apis.Poe.Models.Attribute;
using Item = Sidekick.Business.Parsers.Models.Item;

namespace Sidekick.Business.Parsers
{
    public class ItemParser : IItemParser
    {
        public readonly string[] PROPERTY_SEPERATOR = new string[] { "--------" };
        public readonly string[] NEWLINE_SEPERATOR = new string[] { Environment.NewLine };
        private readonly ILanguageProvider languageProvider;
        private readonly ILogger logger;
        private readonly IMapService mapService;
        private readonly ITokenizer itemNameTokenizer;
        private readonly IAttributeCategoryService attributeCategories;

        private readonly Regex[] AttributeRegexes;
        private readonly Dictionary<string, string> RegexReplacementDictionary;

        public ItemParser(ILanguageProvider languageProvider,
            ILogger logger,
            IEnumerable<ITokenizer> tokenizers,
            IMapService mapService,
            IAttributeCategoryService attributeService)
        {
            this.languageProvider = languageProvider;
            this.logger = logger;
            this.mapService = mapService;
            attributeCategories = attributeService;
            itemNameTokenizer = tokenizers.OfType<ItemNameTokenizer>().First();

            AttributeRegexes = new[] { new Regex(languageProvider.Language.PercentagAddedRegexPattern), new Regex(languageProvider.Language.PercentageIncreasedOrDecreasedRegexPattern),
                                       new Regex(languageProvider.Language.AttributeIncreasedRegexPattern), new Regex(languageProvider.Language.AttributeRangeRegexPattern) };
            RegexReplacementDictionary = new Dictionary<string, string>()
            {
                { languageProvider.Language.PercentagAddedRegexPattern, "#%" },
                { languageProvider.Language.PercentageIncreasedOrDecreasedRegexPattern, "#%" },
                { languageProvider.Language.AttributeIncreasedRegexPattern, "# " },
                { languageProvider.Language.AttributeRangeRegexPattern, "# to # " },
            };
        }

        /// <summary>
        /// Tries to parse an item based on the text that Path of Exile gives on a Ctrl+C action.
        /// There is no recurring logic here so every case has to be handled manually.
        /// </summary>
        public async Task<Item> ParseItem(string itemText, bool parseAttributes = false)
        {
            await languageProvider.FindAndSetLanguage(itemText);

            try
            {
                var lines = itemText.Split(NEWLINE_SEPERATOR, StringSplitOptions.RemoveEmptyEntries);
                // Every item should start with Rarity in the first line.
                if (!lines[0].StartsWith(languageProvider.Language.DescriptionRarity)) throw new Exception("Probably not an item.");

                var itemProperties = GetItemProperties(lines);

                var rarityString = lines[0].Replace(languageProvider.Language.DescriptionRarity, string.Empty);
                var rarity = GetRarity(rarityString);

                Item item;

                if (itemProperties.IsMap)
                {
                    item = GetMapItem(itemProperties, lines, rarity, parseAttributes);
                }
                else if (itemProperties.IsOrgan)
                {
                    item = new OrganItem
                    {
                        Name = lines[1]
                    };
                }
                else
                {
                    item = rarity switch
                    {
                        Rarity.Unique => GetUniqueItem(itemProperties, lines, parseAttributes),
                        Rarity.Rare => GetRareItem(itemProperties, lines, parseAttributes),
                        Rarity.Magic => throw new Exception("Magic items are not yet supported."),
                        Rarity.Normal => GetNormalItem(itemProperties, lines),
                        Rarity.Currency => GetCurrencyItem(lines[1]),
                        Rarity.Gem => GetGemItem(itemProperties, lines),
                        Rarity.DivinationCard => GetDivinationCardItem(lines[1]),
                        _ => throw new NotImplementedException()
                    };
                }

                if (!string.IsNullOrWhiteSpace(item.Name))
                {
                    item.Name = ParseName(item.Name);
                }

                if (!string.IsNullOrWhiteSpace(item.Type))
                {
                    item.Type = ParseName(item.Type);
                }

                item.Rarity = rarity;
                item.IsCorrupted = itemProperties.IsCorrupted;
                item.IsIdentified = itemProperties.IsIdentified;
                item.ItemText = itemText;
                return item;
            }
            catch (Exception e)
            {
                logger.Log("Could not parse item.");
                logger.LogException(e);
                return null;
            }
        }

        private static Item GetDivinationCardItem(string line)
        {
            return new DivinationCardItem()
            {
                Name = line,
                Type = line,
            };
        }

        private Item GetGemItem(ItemProperties itemProperties, string[] lines)
        {
            var item = new GemItem()
            {
                Name = lines[1],        // Need adjustment for Thai Language
                Type = lines[1],        // For Gems the Type has to be set to the Gem Name insead of the name itself
                Level = GetNumberFromString(lines[4]),
                ExperiencePercent = lines.Any(x => x.StartsWith(languageProvider.Language.DescriptionExperience)) ? ParseGemExperiencePercent(lines.Where(y => y.StartsWith(languageProvider.Language.DescriptionExperience)).FirstOrDefault()) : 0, // Some gems have no experience like portal or max ranks
                Quality = itemProperties.HasQuality ? GetNumberFromString(lines.Where(x => x.StartsWith(languageProvider.Language.DescriptionQuality)).FirstOrDefault()) : "0",      // Quality Line Can move for different Gems
                IsVaalVersion = itemProperties.IsCorrupted && lines[3].Contains(languageProvider.Language.KeywordVaal) // check if the gem tags contain Vaal
            };

            // if it's the vaal version, remap to have that name instead
            // Unsure if this works on non arabic lettering (ru/th/kr)
            if (item.IsVaalVersion)
            {
                var vaalName = lines.Where(x => x.Contains(languageProvider.Language.KeywordVaal) && x.Contains(item.Name)).FirstOrDefault(); // this should capture the vaaled name version
                item.Name = vaalName;
                item.Type = vaalName;
            }

            return item;
        }

        private Item GetCurrencyItem(string line)
        {
            return new CurrencyItem()
            {
                Name = line,
                Type = line
            };
        }

        private Item GetNormalItem(ItemProperties itemProperties, string[] lines)
        {
            if (lines.Any(c => c.StartsWith(languageProvider.Language.DescriptionItemLevel))) // Equippable Item
            {
                var item = GetEquippableItem(lines);
                item.Type = lines[1].Replace(languageProvider.Language.PrefixSuperior, string.Empty).Trim();
                item.Name = lines[1].Replace(languageProvider.Language.PrefixSuperior, string.Empty).Trim();

                if (itemProperties.HasNote)
                {
                    item.Influence = GetInfluenceType(lines[lines.Length - 3]);
                }
                else
                {
                    item.Influence = GetInfluenceType(lines.LastOrDefault());
                }

                var links = GetLinkCount(lines.Where(c => c.StartsWith(languageProvider.Language.DescriptionSockets)).FirstOrDefault());

                if (links >= 5)
                {
                    item.Links = new SocketFilterOption()
                    {
                        Min = links,
                        Max = links,
                    };
                }

                return item;
            }
            else if (lines.Any(c => c.Contains(languageProvider.Language.KeywordProphecy))) // Prophecy
            {
                return new ProphecyItem()
                {
                    Name = lines[1],
                    Type = lines[1],
                };
            }
            else // Fragment
            {
                return new FragmentItem()
                {
                    Name = lines[1],
                    Type = lines[1],
                };
            }
        }

        private Item GetRareItem(ItemProperties itemProperties, string[] lines, bool parseAttributes = false)
        {
            var item = GetEquippableItem(lines);
            item.Name = lines[1];
            item.Type = itemProperties.IsIdentified ? lines[2] : lines[1];

            if (parseAttributes)
            {
                item.AttributeDictionary = GetItemAttributes(lines);
            }

            var links = GetLinkCount(lines.Where(c => c.StartsWith(languageProvider.Language.DescriptionSockets)).FirstOrDefault());

            if (links >= 5)
            {
                item.Links = new SocketFilterOption()
                {
                    Min = links,
                    Max = links,
                };
            }

            if (itemProperties.HasNote)
            {
                item.Influence = GetInfluenceType(lines[lines.Length - 3]);
            }
            else
            {
                item.Influence = GetInfluenceType(lines.LastOrDefault());
            }

            return item;
        }

        private EquippableItem GetEquippableItem(string[] lines)
        {
            EquippableItem item;

            // Weapon
            if(lines.Any(c => c.StartsWith(languageProvider.Language.DescriptionPhysicalDamage) || c.StartsWith(languageProvider.Language.DescriptionElementalDamage)))
            {
                // TODO Make Available in Query Parameters
                item = new WeaponItem()
                {
                    AttacksPerSecond = GetNumberFromString(lines.Where(c => c.StartsWith(languageProvider.Language.DescriptionAttacksPerSecond)).FirstOrDefault()),
                    CriticalStrikeChance = GetNumberFromString(lines.Where(c => c.StartsWith(languageProvider.Language.DescriptionCriticalStrikeChance)).FirstOrDefault()),
                    ElementalDps = GetNumberFromString(lines.Where(c => c.StartsWith(languageProvider.Language.DescriptionElementalDamage)).FirstOrDefault(), allowRange: true),
                    PhysicalDps = GetNumberFromString(lines.Where(c => c.StartsWith(languageProvider.Language.DescriptionPhysicalDamage)).FirstOrDefault(), allowRange: true),
                };
            }
            else        // Armour or Jewellery
            {
                item = new ArmourItem()
                {
                    Armour = GetNumberFromString(lines.Where(c => c.StartsWith(languageProvider.Language.DescriptionArmour)).FirstOrDefault()),
                    EnergyShield = GetNumberFromString(lines.Where(c => c.StartsWith(languageProvider.Language.DescriptionEnergyShield)).FirstOrDefault()),
                    Evasion = GetNumberFromString(lines.Where(c => c.StartsWith(languageProvider.Language.DescriptionEvasion)).FirstOrDefault())
                };
            }    

            item.ItemLevel = GetNumberFromString(lines.Where(c => c.StartsWith(languageProvider.Language.DescriptionItemLevel)).FirstOrDefault());

            // TODO Special check for 1 handed weapon or shield

            if(int.Parse(item.ItemLevel) >= 50)
            {
                item.MaxSockets = 6;
            }
            else if(int.Parse(item.ItemLevel) >= 35)
            {
                item.MaxSockets = 5;
            }
            else if(int.Parse(item.ItemLevel) >= 25)
            {
                item.MaxSockets = 4;
            }
            else if(int.Parse(item.ItemLevel) >= 1)
            {
                item.MaxSockets = 3;
            }
            else
            {
                item.MaxSockets = 1;
            }

            return item;
        }

        private Item GetUniqueItem(ItemProperties itemProperties, string[] lines, bool parseAttributes = false)
        {
            var item = GetEquippableItem(lines);

            item.Name = lines[1];
            item.Type = itemProperties.IsIdentified ? lines[2] : lines[1];

            if (parseAttributes)
            {
                item.AttributeDictionary = GetItemAttributes(lines);
            }

            var links = GetLinkCount(lines.Where(c => c.StartsWith(languageProvider.Language.DescriptionSockets)).FirstOrDefault());

            if (links >= 5)
            {
                item.Links = new SocketFilterOption()
                {
                    Min = links,
                    Max = links,
                };
            }

            return item;
        }

        private Item GetMapItem(ItemProperties itemProperties, string[] lines, Rarity rarity, bool parseAttributes = false)
        {
            var item = new MapItem()
            {
                ItemQuantity = GetNumberFromString(lines.Where(c => c.StartsWith(languageProvider.Language.DescriptionItemQuantity)).FirstOrDefault()),
                ItemRarity = GetNumberFromString(lines.Where(c => c.StartsWith(languageProvider.Language.DescriptionItemRarity)).FirstOrDefault()),
                MonsterPackSize = GetNumberFromString(lines.Where(c => c.StartsWith(languageProvider.Language.DescriptionMonsterPackSize)).FirstOrDefault()),
                MapTier = GetNumberFromString(lines.Where(c => c.StartsWith(languageProvider.Language.DescriptionMapTier)).FirstOrDefault()),
                Rarity = rarity,
            };

            if (parseAttributes)
            {
                item.AttributeDictionary = GetItemAttributes(lines);
            }

            if (rarity == Rarity.Normal)
            {
                item.Name = lines[1].Replace(languageProvider.Language.PrefixSuperior, string.Empty).Trim();
                item.Type = lines[1].Replace(languageProvider.Language.PrefixSuperior, string.Empty).Replace(languageProvider.Language.PrefixBlighted, string.Empty).Trim();
            }
            else if (rarity == Rarity.Magic)        // Extract only map name
            {
                item.Name = languageProvider.Language.PrefixBlighted + " " + mapService.MapNames.Where(c => lines[1].Contains(c)).FirstOrDefault();
                item.Type = mapService.MapNames.Where(c => lines[1].Contains(c)).FirstOrDefault();     // Search map name from statics
            }
            else if (rarity == Rarity.Rare)
            {
                item.Name = lines[2].Trim();
                item.Type = lines[2].Replace(languageProvider.Language.PrefixBlighted, string.Empty).Trim();
            }
            else if (rarity == Rarity.Unique)
            {
                if (!itemProperties.IsIdentified)
                {
                    item.Name = lines[1].Replace(languageProvider.Language.PrefixSuperior, string.Empty).Trim();
                }
                else
                {
                    item.Name = lines[1];
                }
            }

            item.IsBlight = itemProperties.IsBlighted.ToString();
            return item;
        }

        private Rarity GetRarity(string rarityString)
        {
            var rarity = Rarity.Unknown;
            if (rarityString == languageProvider.Language.RarityNormal)
            {
                rarity = Rarity.Normal;
            }
            else if (rarityString == languageProvider.Language.RarityMagic)
            {
                rarity = Rarity.Magic;
            }
            else if (rarityString == languageProvider.Language.RarityRare)
            {
                rarity = Rarity.Rare;
            }
            else if (rarityString == languageProvider.Language.RarityUnique)
            {
                rarity = Rarity.Unique;
            }
            else if (rarityString == languageProvider.Language.RarityCurrency)
            {
                rarity = Rarity.Currency;
            }
            else if (rarityString == languageProvider.Language.RarityGem)
            {
                rarity = Rarity.Gem;
            }
            else if (rarityString == languageProvider.Language.RarityDivinationCard)
            {
                rarity = Rarity.DivinationCard;
            }

            return rarity;
        }

        private ItemProperties GetItemProperties(string[] lines)
        {
            var properties = new ItemProperties
            {
                IsIdentified = true
            };

            for (var i = 0; i < lines.Length; i++)
            {
                var line = lines[i];
                if (line == languageProvider.Language.DescriptionUnidentified)
                {
                    properties.IsIdentified = false;
                }
                else if (line.Contains(languageProvider.Language.DescriptionQuality))
                {
                    properties.HasQuality = true;
                }
                else if (line == languageProvider.Language.DescriptionCorrupted)
                {
                    properties.IsCorrupted = true;
                }
                else if (line.Contains(languageProvider.Language.DescriptionMapTier))
                {
                    properties.IsMap = true;
                }
                else if (line.Contains(languageProvider.Language.PrefixBlighted))
                {
                    properties.IsBlighted = true;
                }
                else if (line.Contains(languageProvider.Language.DescriptionOrgan))
                {
                    properties.IsOrgan = true;
                }
                else if (i == lines.Length - 1 && line.Contains("Note"))
                {
                    properties.HasNote = true;
                }
            }

            return properties;
        }

        private string ParseName(string name)
        {
            var langs = new List<string>();
            var tokens = itemNameTokenizer.Tokenize(name);
            var output = "";

            foreach (var token in tokens.Select(x => x as ItemNameToken))
            {
                if (token.TokenType == ItemNameTokenType.Set)
                {
                    langs.Add(token.Match.Match.Groups["LANG"].Value);
                }
                else if (token.TokenType == ItemNameTokenType.Name)
                {
                    output += token.Match.Match.Value;
                }
                else if (token.TokenType == ItemNameTokenType.If)
                {
                    var lang = token.Match.Match.Groups["LANG"].Value;
                    if (langs.Contains(lang))
                        output += token.Match.Match.Groups["NAME"].Value;
                }
            }

            return output;
        }

        private string GetNumberFromString(string input, bool allowNegative = false, bool allowRange = false)
        {
            if (string.IsNullOrEmpty(input))     // Default return 0
            {
                return "0";
            }

            string numberStr;

            if (allowRange)
            {
                numberStr = new string(input.Where(c => char.IsDigit(c) || c == '.' || c == '-').ToArray());
            }
            else
            {
                // Also parse double values
                numberStr = new string(input.Where(c => char.IsDigit(c) || c == '.').ToArray());
            }

            if (allowNegative)
            {
                if (input.StartsWith("-"))
                {
                    return "-" + numberStr;
                }
            }

            return numberStr;
        }

        private (int? min, int? max) GetNumberRange(string input)
        {
            if (!input.Contains(languageProvider.Language.KeywordRange))
            {
                var numberStr = GetNumberFromString(input, true);

                if (!int.TryParse(numberStr, out var value))
                {
                    return (null, null);
                }

                return (value, null);
            }

            int index = input.IndexOf(languageProvider.Language.KeywordRange);

            string subStr1 = GetNumberFromString(input.Substring(0, index));
            string subStr2 = GetNumberFromString(input.Substring(index + 1, input.Length - index - 1));

            int? value1 = null;
            int? value2 = null;

            if (int.TryParse(subStr1, out var result))
            {
                value1 = result;
            }

            if (int.TryParse(subStr2, out result))
            {
                value2 = result;
            }

            return (value1, value2);
        }

        private int ParseGemExperiencePercent(string input)
        {
            // trim leading prefix if any
            if (input.Contains(languageProvider.Language.DescriptionExperience))
                input = input.Replace(languageProvider.Language.DescriptionExperience, "");
            var split = input.Split('/');

            int percent;
            if (split.Length == 2)
            {
                int.TryParse(split[0], out var current);
                int.TryParse(split[1], out var max);

                percent = (int)(((float)current / max) * 100.0f);
                percent = (percent < 100) ? percent : 100;
            }
            else
            {
                throw new Exception("unable to parse gem experience from input string: " + input);
            }

            return percent;
        }

        private int GetLinkCount(string input)
        {
            if (input == null || !input.StartsWith(languageProvider.Language.DescriptionSockets))
            {
                return 0;
            }

            var values = new List<int>();

            if (!string.IsNullOrEmpty(input))
            {
                foreach (var fragment in input.Split(' '))
                {
                    values.Add(fragment.Count(c => c == '-') == 0 ? 0 : fragment.Count(c => c == '-') + 1);
                }

                return values.Max();
            }
            else
            {
                return 0;
            }
        }

        private InfluenceType GetInfluenceType(string input)
        {
            if (input.Contains(languageProvider.Language.InfluenceShaper))
            {
                return InfluenceType.Shaper;
            }
            else if (input.Contains(languageProvider.Language.InfluenceElder))
            {
                return InfluenceType.Elder;
            }
            else if (input.Contains(languageProvider.Language.InfluenceCrusader))
            {
                return InfluenceType.Crusader;
            }
            else if (input.Contains(languageProvider.Language.InfluenceHunter))
            {
                return InfluenceType.Hunter;
            }
            else if (input.Contains(languageProvider.Language.InfluenceRedeemer))
            {
                return InfluenceType.Redeemer;
            }
            else if (input.Contains(languageProvider.Language.InfluenceWarlord))
            {
                return InfluenceType.Warlord;
            }

            return InfluenceType.None;
        }

        private Dictionary<Attribute, FilterValue> GetItemAttributes(IEnumerable<string> input)
        {
            var result = new Dictionary<Attribute, FilterValue>();

            foreach (var line in input)
            {
                string formattedLine = line;
                string formattedValue = line;

                foreach (var regex in AttributeRegexes)
                {
                    if (regex.IsMatch(formattedLine))
                    {
                        var match = regex.Match(formattedLine);
                        formattedValue = match.Value;
                        formattedLine = regex.Replace(formattedLine, RegexReplacementDictionary[regex.ToString()]);
                        break;
                    }
                }

                List<Attribute> attributesToSearch;

#warning TODO Find a better way for special case
                if (formattedLine.StartsWith("#% increased Energy Shield"))
                {
                    formattedLine = formattedLine + " (Local)";
                }

                if (formattedLine.Contains(languageProvider.Language.CategoryNameCrafted))
                {
                    formattedLine = formattedLine.Replace(languageProvider.Language.CategoryNameCrafted, "").Trim();
                    attributesToSearch = attributeCategories.Categories.Where(c => c.Label == languageProvider.Language.AttributeCategoryCrafted).SelectMany(c => c.Entries).ToList();
                }
                else if (formattedLine.Contains(languageProvider.Language.CategoryNameFractured))
                {
                    formattedLine = formattedLine.Replace(languageProvider.Language.CategoryNameFractured, "").Trim();
                    attributesToSearch = attributeCategories.Categories.Where(c => c.Label == languageProvider.Language.AttributeCategoryFractured).SelectMany(c => c.Entries).ToList();
                }
                else if (formattedLine.Contains(languageProvider.Language.CategoryNameImplicit))
                {
                    formattedLine = formattedLine.Replace(languageProvider.Language.CategoryNameImplicit, "").Trim();
                    attributesToSearch = attributeCategories.Categories.Where(c => c.Label == languageProvider.Language.AttributeCategoryImplicit).SelectMany(c => c.Entries).ToList();
                }
                else
                {
                    attributesToSearch = attributeCategories.Categories.Where(c => c.Label == languageProvider.Language.AttributeCategoryExplicit ||
                                                                                   c.Label == languageProvider.Language.AttributeCategoryDelve ||
                                                                                   c.Label == languageProvider.Language.AttributeCategoryEnchant ||
                                                                                   c.Label == languageProvider.Language.AttributeCategoryVeiled).SelectMany(c => c.Entries).ToList();
                }

                var attribute = attributesToSearch.Where(c => c.Text == formattedLine).FirstOrDefault();

                if (attribute != null)
                {
                    var attrValues = GetNumberRange(formattedValue);
                    result.Add(attribute, new FilterValue() { Min = attrValues.min, Max = attrValues.max });
                }
            }

            return result;
        }
    }
}
