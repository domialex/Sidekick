using System;
using System.Collections.Generic;
using System.Linq;
using Sidekick.Business.Filters;
using Sidekick.Business.Languages;
using Sidekick.Business.Loggers;
using Sidekick.Business.Parsers.Models;
using Sidekick.Business.Parsers.Types;
using Sidekick.Business.Tokenizers.ItemName;
using Sidekick.Business.Trades;

namespace Sidekick.Business.Parsers
{
    public class ItemParser : IItemParser
    {
        public readonly string[] PROPERTY_SEPERATOR = new string[] { "--------" };
        public readonly string[] NEWLINE_SEPERATOR = new string[] { Environment.NewLine };
        private readonly Tokenizer itemNameTokenizer = new Tokenizer();
        private readonly ILanguageProvider languageProvider;
        private readonly ILogger logger;
        private readonly ITradeClient tradeClient;

        public ItemParser(ILanguageProvider languageProvider, ILogger logger, ITradeClient tradeClient)
        {
            this.languageProvider = languageProvider;
            this.logger = logger;
            this.tradeClient = tradeClient;
        }

        /// <summary>
        /// Tries to parse an item based on the text that Path of Exile gives on a Ctrl+C action.
        /// There is no recurring logic here so every case has to be handled manually.
        /// </summary>
        public Item ParseItem(string text)
        {
            Item item = null;
            bool isIdentified, hasQuality, isCorrupted, isMap, isBlighted, hasNote;

            try
            {
                var lines = text.Split(NEWLINE_SEPERATOR, StringSplitOptions.RemoveEmptyEntries);
                // Every item should start with Rarity in the first line.
                if (!lines[0].StartsWith(languageProvider.Language.DescriptionRarity)) throw new Exception("Probably not an item.");
                // If the item is Unidentified, the second line will be its Type instead of the Name.
                isIdentified = !lines.Any(x => x == languageProvider.Language.DescriptionUnidentified);
                hasQuality = lines.Any(x => x.Contains(languageProvider.Language.DescriptionQuality));
                isCorrupted = lines.Any(x => x == languageProvider.Language.DescriptionCorrupted);
                isMap = lines.Any(x => x.Contains(languageProvider.Language.DescriptionMapTier));
                isBlighted = lines.Any(x => x.Contains(languageProvider.Language.PrefixBlighted));
                hasNote = lines.LastOrDefault().Contains("Note");

                var rarity = lines[0].Replace(languageProvider.Language.DescriptionRarity, string.Empty);

                if (isMap)
                {
                    item = new MapItem()
                    {
                        ItemQuantity = GetNumberFromString(lines.Where(c => c.StartsWith(languageProvider.Language.DescriptionItemQuantity)).FirstOrDefault()),
                        ItemRarity = GetNumberFromString(lines.Where(c => c.StartsWith(languageProvider.Language.DescriptionItemRarity)).FirstOrDefault()),
                        MonsterPackSize = GetNumberFromString(lines.Where(c => c.StartsWith(languageProvider.Language.DescriptionMonsterPackSize)).FirstOrDefault()),
                        MapTier = GetNumberFromString(lines.Where(c => c.StartsWith(languageProvider.Language.DescriptionMapTier)).FirstOrDefault()),
                        Rarity = rarity,
                    };

                    if (rarity == languageProvider.Language.RarityNormal)
                    {
                        item.Name = lines[1].Replace(languageProvider.Language.PrefixSuperior, string.Empty).Trim();
                        item.Type = lines[1].Replace(languageProvider.Language.PrefixSuperior, string.Empty).Replace(languageProvider.Language.PrefixBlighted, string.Empty).Trim();
                    }
                    else if (rarity == languageProvider.Language.RarityMagic)        // Extract only map name
                    {
                        item.Name = languageProvider.Language.PrefixBlighted + " " + tradeClient.MapNames.Where(c => lines[1].Contains(c)).FirstOrDefault();
                        item.Type = tradeClient.MapNames.Where(c => lines[1].Contains(c)).FirstOrDefault();     // Search map name from statics
                    }
                    else if (rarity == languageProvider.Language.RarityRare)
                    {
                        item.Name = lines[2].Trim();
                        item.Type = lines[2].Replace(languageProvider.Language.PrefixBlighted, string.Empty).Trim();
                    }
                    else if (rarity == languageProvider.Language.RarityUnique)
                    {
                        if (!isIdentified)
                        {
                            item.Name = lines[1].Replace(languageProvider.Language.PrefixSuperior, string.Empty).Trim();
                        }
                        else
                        {
                            item.Name = lines[1];
                        }
                    }

                    ((MapItem)item).IsBlight = isBlighted ? "true" : "false";
                }
                else if (rarity == languageProvider.Language.RarityUnique)
                {
                    item = new EquippableItem
                    {
                        Name = lines[1],
                        Type = isIdentified ? lines[2] : lines[1]
                    };

                    var links = GetLinkCount(lines.Where(c => c.StartsWith(languageProvider.Language.DescriptionSockets)).FirstOrDefault());

                    if (links >= 5)
                    {
                        ((EquippableItem)item).Links = new SocketFilterOption()
                        {
                            Min = links,
                            Max = links,
                        };
                    }
                }
                else if (rarity == languageProvider.Language.RarityRare)
                {
                    item = new EquippableItem()
                    {
                        Name = lines[1],
                        Type = lines[2],
                        ItemLevel = GetNumberFromString(lines.Where(c => c.StartsWith(languageProvider.Language.DescriptionItemLevel)).FirstOrDefault()),
                    };

                    var links = GetLinkCount(lines.Where(c => c.StartsWith(languageProvider.Language.DescriptionSockets)).FirstOrDefault());

                    if (links >= 5)
                    {
                        ((EquippableItem)item).Links = new SocketFilterOption()
                        {
                            Min = links,
                            Max = links,
                        };
                    }

                    if (hasNote)
                    {
                        ((EquippableItem)item).Influence = GetInfluenceType(lines[lines.Length - 3]);
                    }
                    else
                    {
                        ((EquippableItem)item).Influence = GetInfluenceType(lines.LastOrDefault());
                    }
                }
                else if (rarity == languageProvider.Language.RarityMagic)
                {
                    throw new Exception("Magic items are not yet supported.");
                }
                else if (rarity == languageProvider.Language.RarityNormal)
                {
                    if (lines.Any(c => c.StartsWith(languageProvider.Language.DescriptionItemLevel)))      // Equippable Item
                    {
                        item = new EquippableItem()
                        {
                            Type = lines[1].Replace(languageProvider.Language.PrefixSuperior, string.Empty).Trim(),
                            Name = lines[1].Replace(languageProvider.Language.PrefixSuperior, string.Empty).Trim(),
                            ItemLevel = GetNumberFromString(lines.Where(c => c.StartsWith(languageProvider.Language.DescriptionItemLevel)).FirstOrDefault()),
                        };

                        if (hasNote)
                        {
                            ((EquippableItem)item).Influence = GetInfluenceType(lines[lines.Length - 3]);
                        }
                        else
                        {
                            ((EquippableItem)item).Influence = GetInfluenceType(lines.LastOrDefault());
                        }

                        var links = GetLinkCount(lines.Where(c => c.StartsWith(languageProvider.Language.DescriptionSockets)).FirstOrDefault());

                        if (links >= 5)
                        {
                            ((EquippableItem)item).Links = new SocketFilterOption()
                            {
                                Min = links,
                                Max = links,
                            };
                        }
                    }
                    else if (lines.Any(c => c.Contains(languageProvider.Language.KeywordProphecy)))      // Prophecy
                    {
                        item = new ProphecyItem()
                        {
                            Name = lines[1],
                            Type = lines[1],
                        };
                    }
                    else                // Fragment
                    {
                        item = new FragmentItem()
                        {
                            Name = lines[1],
                            Type = lines[1],
                        };
                    }
                }
                else if (rarity == languageProvider.Language.RarityCurrency)
                {
                    item = new CurrencyItem()
                    {
                        Name = lines[1],
                        Type = lines[1]
                    };
                }
                else if (rarity == languageProvider.Language.RarityGem)
                {
                    item = new GemItem()
                    {
                        Name = lines[1],        // Need adjustment for Thai Language
                        Type = lines[1],        // For Gems the Type has to be set to the Gem Name insead of the name itself
                        Level = GetNumberFromString(lines[4]),
                        ExperiencePercent = lines.Any(x => x.StartsWith(languageProvider.Language.DescriptionExperience)) ? ParseGemExperiencePercent(lines.Where(y => y.StartsWith(languageProvider.Language.DescriptionExperience)).FirstOrDefault()) : 0, // Some gems have no experience like portal or max ranks
                        Quality = hasQuality ? GetNumberFromString(lines.Where(x => x.StartsWith(languageProvider.Language.DescriptionQuality)).FirstOrDefault()) : "0",      // Quality Line Can move for different Gems
                        IsVaalVersion = isCorrupted && lines[3].Contains(languageProvider.Language.KeywordVaal) // check if the gem tags contain Vaal
                    };

                    // if it's the vaal version, remap to have that name instead
                    // Unsure if this works on non arabic lettering (ru/th/kr)
                    if ((item as GemItem).IsVaalVersion)
                    {
                        string vaalName = lines.Where(x => x.Contains(languageProvider.Language.KeywordVaal) && x.Contains(item.Name)).FirstOrDefault(); // this should capture the vaaled name version
                        item.Name = vaalName;
                        item.Type = vaalName;
                    }
                }
                else if (rarity == languageProvider.Language.RarityDivinationCard)
                {
                    item = new DivinationCardItem()
                    {
                        Name = lines[1],
                        Type = lines[1],
                    };
                }
                else
                {
                    throw new NotImplementedException();
                }

                if (item != null && string.IsNullOrEmpty(item.Rarity))
                {
                    item.Rarity = string.IsNullOrEmpty(rarity) ? "unknown" : rarity;
                }

                if (!string.IsNullOrWhiteSpace(item.Name))
                    item.Name = ParseName(item.Name);

                if (!string.IsNullOrWhiteSpace(item.Type))
                    item.Type = ParseName(item.Type);
            }
            catch (Exception e)
            {
                logger.Log("Could not parse item. " + e.Message);
                return null;
            }

            item.IsCorrupted = isCorrupted;
            return item;
        }

        private string ParseName(string name)
        {
            var langs = new List<string>();
            var tokens = itemNameTokenizer.Tokenize(name);
            var output = "";

            foreach (var token in tokens)
            {
                if (token.TokenType == TokenType.Set)
                    langs.Add(token.Match.Match.Groups["LANG"].Value);
                else if (token.TokenType == TokenType.Name)
                    output += token.Match.Match.Value;
                else if (token.TokenType == TokenType.If)
                {
                    var lang = token.Match.Match.Groups["LANG"].Value;
                    if (langs.Contains(lang))
                        output += token.Match.Match.Groups["NAME"].Value;
                }
            }

            return output;
        }

        private string GetNumberFromString(string input)
        {
            if (string.IsNullOrEmpty(input))     // Default return 0
            {
                return "0";
            }

            return new string(input.Where(c => char.IsDigit(c)).ToArray());
        }

        private int ParseGemExperiencePercent(string input)
        {
            // trim leading prefix if any
            if (input.Contains(languageProvider.Language.DescriptionExperience))
                input = input.Replace(languageProvider.Language.DescriptionExperience, "");

            int percent = 0;
            var split = input.Split('/');
            if (split.Length == 2)
            {
                int max = 1; // no division by 0
                int.TryParse(split[0], out int current);
                int.TryParse(split[1], out max);

                percent = (int)(((float)current / (float)max) * 100.0f);
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

            List<int> values = new List<int>();

            if (!string.IsNullOrEmpty(input))
            {
                foreach (string fragment in input.Split(' '))
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

        internal InfluenceType GetInfluenceType(string input)
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
    }
}
