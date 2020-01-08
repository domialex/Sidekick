using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sidekick.Helpers.POETradeAPI.Models;
using System.Text.RegularExpressions;
using Sidekick.Localization;

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
            bool isIdentified, hasQuality, isCorrupted;

            try
            {
                var lines = text.Split(NEWLINE_SEPERATOR, StringSplitOptions.RemoveEmptyEntries);
                // Every item should start with Rarity in the first line.
                if (!lines[0].StartsWith(LanguageSettings.Provider.DescriptionRarity)) throw new Exception("Probably not an item.");
                // If the item is Unidentified, the second line will be its Type instead of the Name.
                isIdentified = !lines.Any(x => x == LanguageSettings.Provider.DescriptionUnidentified);
                hasQuality = lines.Any(x => x.Contains(LanguageSettings.Provider.DescriptionQuality));
                isCorrupted = lines.Any(x => x == LanguageSettings.Provider.DescriptionCorrupted);

                var rarity = lines[0].Replace(LanguageSettings.Provider.DescriptionRarity, string.Empty);

                if(rarity == LanguageSettings.Provider.RarityUnique)
                {
                    item = new EquippableItem();

                    if (isIdentified)
                    {
                        item.Name = lines[1];
                        item.Type = lines[2];
                    }
                    else
                    {
                        item.Type = lines[1];
                    }

                    var links = GetLinkCount(lines.Where(c => c.StartsWith(LanguageSettings.Provider.DescriptionSockets)).FirstOrDefault());

                    if (links >= 5)
                    {
                        ((EquippableItem)item).Links = new SocketFilterOption()
                        {
                            Min = links,
                            Max = links,
                        };
                    }

                    ((EquippableItem)item).Rarity = rarity;
                }
                else if(rarity == LanguageSettings.Provider.RarityRare)
                {
                    item = new EquippableItem()
                    {
                        Name = lines[2].Replace(LanguageSettings.Provider.PrefixSuperior, string.Empty),
                        Type = lines[2].Replace(LanguageSettings.Provider.PrefixSuperior, string.Empty),
                        ItemLevel = GetNumberFromString(lines.Where(c => c.StartsWith(LanguageSettings.Provider.DescriptionItemLevel)).FirstOrDefault()),
                        Rarity = rarity,
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
                            Type = lines[1].Replace(LanguageSettings.Provider.PrefixSuperior, string.Empty),
                            ItemLevel = GetNumberFromString(lines.Where(c => c.StartsWith(LanguageSettings.Provider.DescriptionItemLevel)).FirstOrDefault()),
                            Rarity = rarity,                           // TODO Non-Unique Rarity
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
                    else                // Fragment
                    {
                        item = new FragmentItem()
                        {
                            Type = lines[1],
                        };
                    }
                }
                else if(rarity == LanguageSettings.Provider.RarityCurrency)
                {
                    item = new CurrencyItem()
                    {
                        Name = lines[1]
                    };

                }
                else if(rarity == LanguageSettings.Provider.RarityGem)
                {
                    item = new GemItem()
                    {
                        Type = lines[1],        // For Gems the Type has to be set to the Gem Name insead of the name itself
                        Level = GetNumberFromString(lines[4]),
                        Quality = hasQuality ? GetNumberFromString(lines.Where(x => x.StartsWith(LanguageSettings.Provider.DescriptionQuality)).FirstOrDefault()) : "0",      // Quality Line Can move for different Gems
                    };
                }
                else if(rarity == LanguageSettings.Provider.RarityDivinationCard)
                {
                    item = new CurrencyItem()
                    {
                        Name = lines[1],
                        Type = lines[1],
                    };
                }
                else
                {
                    throw new NotImplementedException();
                }
              
            }
            catch (Exception e)
            {
                Logger.Log("Could not parse item. " + e.Message);
                return null;
            }

            item.IsCorrupted = isCorrupted ? "true" : "false";
            return item;
        }

        internal static string GetNumberFromString(string input)
        {
            return new string(input.Where(c => char.IsDigit(c)).ToArray());
        }

        internal static int GetLinkCount(string input)
        {
            if(input == null || !input.StartsWith(LanguageSettings.Provider.DescriptionSockets))
            {
                return 0;
            }

            List<int> values = new List<int>();

            if (!String.IsNullOrEmpty(input))
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
            if(input == LanguageSettings.Provider.InfluenceShaper)
            {
                return InfluenceType.Shaper;
            }
            else if(input == LanguageSettings.Provider.InfluenceElder)
            {
                return InfluenceType.Elder;
            }
            else if(input == LanguageSettings.Provider.InfluenceCrusader)
            {
                return InfluenceType.Crusader;
            }
            else if(input == LanguageSettings.Provider.InfluenceHunter)
            {
                return InfluenceType.Hunter;
            }
            else if(input == LanguageSettings.Provider.InfluenceRedeemer)
            {
                return InfluenceType.Redeemer;
            }
            else if(input == LanguageSettings.Provider.InfluenceWarlord)
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
    }

    public class EquippableItem : Item
    {
        public string Rarity { get; set; }
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
        // IsVaalVersion
    }

    public class CurrencyItem : Item
    {
    }

    public class FragmentItem : Item
    {
    }
}
