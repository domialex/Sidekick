using System;
using System.Collections.Generic;
using System.Linq;
using Sidekick.Helpers.POETradeAPI.Models;
using Sidekick.Helpers.Localization;
using Sidekick.Helpers.POETradeAPI;

namespace Sidekick.Helpers
{
    public static class ItemParser
    {
        private static readonly string[] PROPERTY_SEPERATOR = new string[] { "--------" };
        private static readonly string[] NEWLINE_SEPERATOR = new string[] { Environment.NewLine };

        /// <summary>
        /// Tries to parse an item based on the text that Path of Exile gives on a Ctrl+C action.
        /// There is no recurring logic here so every case has to be handled manually.
        /// </summary>
        public static Item ParseItem(string text)
        {
            Item item = null;
            bool isIdentified, hasQuality, isCorrupted, isMap, isBlighted;

            try
            {
                var lines = text.Split(NEWLINE_SEPERATOR, StringSplitOptions.RemoveEmptyEntries);
                // Every item should start with Rarity in the first line.
                if (!lines[0].StartsWith(LanguageSettings.Provider.DescriptionRarity)) throw new Exception("Probably not an item.");
                // If the item is Unidentified, the second line will be its Type instead of the Name.
                isIdentified = !lines.Any(x => x == LanguageSettings.Provider.DescriptionUnidentified);
                hasQuality = lines.Any(x => x.Contains(LanguageSettings.Provider.DescriptionQuality));
                isCorrupted = lines.Any(x => x == LanguageSettings.Provider.DescriptionCorrupted);
                isMap = lines.Any(x => x.Contains(LanguageSettings.Provider.DescriptionMapTier));
                isBlighted = lines.Any(x => x.Contains(LanguageSettings.Provider.PrefixBlighted));

                var rarity = lines[0].Replace(LanguageSettings.Provider.DescriptionRarity, string.Empty);

                if(isMap)
                {
                    item = new MapItem()
                    {
                        ItemQuantity = GetNumberFromString(lines.Where(c => c.StartsWith(LanguageSettings.Provider.DescriptionItemQuantity)).FirstOrDefault()),
                        ItemRarity = GetNumberFromString(lines.Where(c => c.StartsWith(LanguageSettings.Provider.DescriptionItemRarity)).FirstOrDefault()),
                        MonsterPackSize = GetNumberFromString(lines.Where(c => c.StartsWith(LanguageSettings.Provider.DescriptionMonsterPackSize)).FirstOrDefault()),
                        MapTier = GetNumberFromString(lines.Where(c => c.StartsWith(LanguageSettings.Provider.DescriptionMapTier)).FirstOrDefault()),
                        Rarity = rarity,
                    };

                    if(rarity == LanguageSettings.Provider.RarityNormal)
                    {
                        item.Name = lines[1].Replace(LanguageSettings.Provider.PrefixSuperior, string.Empty).Trim();
                        item.Type = lines[1].Replace(LanguageSettings.Provider.PrefixSuperior, string.Empty).Replace(LanguageSettings.Provider.PrefixBlighted, string.Empty).Trim();
                    }
                    else if(rarity == LanguageSettings.Provider.RarityMagic)        // Extract only map name
                    {
                        item.Name = LanguageSettings.Provider.PrefixBlighted + " " + TradeClient.MapNames.Where(c => lines[1].Contains(c)).FirstOrDefault();
                        item.Type = TradeClient.MapNames.Where(c => lines[1].Contains(c)).FirstOrDefault();     // Search map name from statics
                    }
                    else if(rarity == LanguageSettings.Provider.RarityRare)
                    {
                        item.Name = lines[2].Trim();
                        item.Type = lines[2].Replace(LanguageSettings.Provider.PrefixBlighted, string.Empty).Trim();
                    }
                    else if(rarity == LanguageSettings.Provider.RarityUnique)
                    {
                        if (!isIdentified)
                        {
                            item.Name = lines[1].Replace(LanguageSettings.Provider.PrefixSuperior, string.Empty).Trim();
                        }
                        else
                        {
                            item.Name = lines[1];
                        }
                    }

                    ((MapItem)item).IsBlight = isBlighted ? "true" : "false";
                }
                else if(rarity == LanguageSettings.Provider.RarityUnique)
                {
                    item = new EquippableItem
                    {
                        Name = lines[1],
                        Type = isIdentified ? lines[2] : lines[1]
                    };

                    var links = GetLinkCount(lines.Where(c => c.StartsWith(LanguageSettings.Provider.DescriptionSockets)).FirstOrDefault());

                    if (links >= 5)
                    {
                        ((EquippableItem)item).Links = new SocketFilterOption()
                        {
                            Min = links,
                            Max = links,
                        };
                    }
                }
                else if(rarity == LanguageSettings.Provider.RarityRare)
                {
                    item = new EquippableItem()
                    {
                    	Name = lines[1],
                    	Type = lines[2],
                        ItemLevel = GetNumberFromString(lines.Where(c => c.StartsWith(LanguageSettings.Provider.DescriptionItemLevel)).FirstOrDefault()),
                    };

                    var links = GetLinkCount(lines.Where(c => c.StartsWith(LanguageSettings.Provider.DescriptionSockets)).FirstOrDefault());

                    if (links >= 5)
                    {
                        ((EquippableItem)item).Links = new SocketFilterOption()
                        {
                            Min = links,
                            Max = links,
                        };
                    }

                    var influence = GetInfluenceType(lines.LastOrDefault());

                    ((EquippableItem)item).Influence = influence;
                }
                else if(rarity == LanguageSettings.Provider.RarityMagic)
                {
                    throw new Exception("Magic items not supported for now.");
                }
                else if(rarity == LanguageSettings.Provider.RarityNormal)
                {
                    if (lines.Any(c => c.StartsWith(LanguageSettings.Provider.DescriptionItemLevel)))      // Equippable Item
                    {
                        item = new EquippableItem()
                        {
                            Type = lines[1].Replace(LanguageSettings.Provider.PrefixSuperior, string.Empty).Trim(),
							Name = lines[1].Replace(LanguageSettings.Provider.PrefixSuperior, string.Empty).Trim(),
                            ItemLevel = GetNumberFromString(lines.Where(c => c.StartsWith(LanguageSettings.Provider.DescriptionItemLevel)).FirstOrDefault()),
                        };

                        var influence = GetInfluenceType(lines.LastOrDefault());
                        ((EquippableItem)item).Influence = influence;

                        var links = GetLinkCount(lines.Where(c => c.StartsWith(LanguageSettings.Provider.DescriptionSockets)).FirstOrDefault());

                        if (links >= 5)
                        {
                            ((EquippableItem)item).Links = new SocketFilterOption()
                            {
                                Min = links,
                                Max = links,
                            };
                        }
                    }
                    else if(lines.Any(c => c.Contains(LanguageSettings.Provider.KeywordProphecy)))      // Prophecy
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
                else if(rarity == LanguageSettings.Provider.RarityCurrency)
                {
                    item = new CurrencyItem()
                    {
                        Name = lines[1],
                        Type = lines[1]
                    };
                }
                else if(rarity == LanguageSettings.Provider.RarityGem)
                {
                    item = new GemItem()
                    {
                        Name = lines[1],        // Need adjustment for Thai Language
                        Type = lines[1],        // For Gems the Type has to be set to the Gem Name insead of the name itself
                        Level = GetNumberFromString(lines[4]),
                        ExperiencePercent = lines.Any(x => x.StartsWith(LanguageSettings.Provider.DescriptionExperience)) ? ParseGemExperiencePercent(lines.Where(y => y.StartsWith(LanguageSettings.Provider.DescriptionExperience)).FirstOrDefault()) : 0, // Some gems have no experience like portal or max ranks
                        Quality = hasQuality ? GetNumberFromString(lines.Where(x => x.StartsWith(LanguageSettings.Provider.DescriptionQuality)).FirstOrDefault()) : "0",      // Quality Line Can move for different Gems
                        IsVaalVersion = isCorrupted && lines[3].Contains(LanguageSettings.Provider.KeywordVaal) // check if the gem tags contain Vaal
                    };

                    // if it's the vaal version, remap to have that name instead
                    // Unsure if this works on non arabic lettering (ru/th/kr)
                    if ((item as GemItem).IsVaalVersion)
                    {
                        string vaalName = lines.Where(x => x.Contains(LanguageSettings.Provider.KeywordVaal) && x.Contains(item.Name)).FirstOrDefault(); // this should capture the vaaled name version
                        item.Name = vaalName;
                        item.Type = vaalName;
                    }
                }
                else if(rarity == LanguageSettings.Provider.RarityDivinationCard)
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
              
                if(item != null && string.IsNullOrEmpty(item.Rarity))
                {
                    item.Rarity = string.IsNullOrEmpty(rarity) ? "unknown" : rarity;
                }
            }
            catch (Exception e)
            {
                Logger.LogException("Could not parse item", e);
                return null;
            }

            item.IsCorrupted = isCorrupted ? "true" : "false";
            return item;
        }

        internal static string GetNumberFromString(string input)
        {
            if(string.IsNullOrEmpty(input))     // Default return 0
            {
                return "0";
            }

            return new string(input.Where(c => char.IsDigit(c)).ToArray());
        }

        internal static int ParseGemExperiencePercent(string input)
        {
            // trim leading prefix if any
            if (input.Contains(LanguageSettings.Provider.DescriptionExperience))
                input = input.Replace(LanguageSettings.Provider.DescriptionExperience, "");

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

        internal static int GetLinkCount(string input)
        {
            if(input == null || !input.StartsWith(LanguageSettings.Provider.DescriptionSockets))
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

        internal static InfluenceType GetInfluenceType(string input)
        {
            if(input.Contains(LanguageSettings.Provider.InfluenceShaper))
            {
                return InfluenceType.Shaper;
            }
            else if(input.Contains(LanguageSettings.Provider.InfluenceElder))
            {
                return InfluenceType.Elder;
            }
            else if(input.Contains(LanguageSettings.Provider.InfluenceCrusader))
            {
                return InfluenceType.Crusader;
            }
            else if(input.Contains(LanguageSettings.Provider.InfluenceHunter))
            {
                return InfluenceType.Hunter;
            }
            else if(input.Contains(LanguageSettings.Provider.InfluenceRedeemer))
            {
                return InfluenceType.Redeemer;
            }
            else if(input.Contains(LanguageSettings.Provider.InfluenceWarlord))
            {
                return InfluenceType.Warlord;
            }

            return InfluenceType.None;
        }
    }

    public abstract class Item
    {
        public string Name { get; set; }
        public string Type { get; set; }
        public string IsCorrupted { get; set; }
        public string Rarity { get; set; }
    }

    public class EquippableItem : Item
    {
        public string Quality { get; set; }
        public string ItemLevel { get; set; }
        public InfluenceType Influence { get; set; }
        public SocketFilterOption Sockets { get; set; }
        public SocketFilterOption Links { get; set; }
    }

    public class GemItem : Item
    {
        public string Level { get; set; }
        public string Quality { get; set; }
        public bool IsVaalVersion { get; set; }
        public int ExperiencePercent { get; set; } // percent towards next level
    }

    public class CurrencyItem : Item
    {
    }

    public class DivinationCardItem : Item
    {
    }

    public class FragmentItem : Item
    {
    }

    public class ProphecyItem : Item
    {
    }

    public class MapItem : Item
    {
        public string MapTier { get; set; }
        public string ItemQuantity { get; set; }
        public string ItemRarity { get; set; }
        public string MonsterPackSize { get; set; }
        public string IsBlight { get; set; }
    }
}
