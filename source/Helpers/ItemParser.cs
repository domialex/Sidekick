using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sidekick.Helpers.POETradeAPI.Models;
using System.Text.RegularExpressions;

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
            bool isIdentitied, hasQuality, isCorrupted;

            try
            {
                var lines = text.Split(NEWLINE_SEPERATOR, StringSplitOptions.RemoveEmptyEntries);
                // Every item should start with Rarity in the first line.
                if (!lines[0].StartsWith("Rarity: ")) throw new Exception("Probably not an item.");
                // If the item is Unidentified, the second line will be its Type instead of the Name.
                isIdentitied = !lines.Any(x => x == StringConstants.DescriptionUnidentified);
                hasQuality = lines.Any(x => x.Contains(StringConstants.DescriptionQuality));
                isCorrupted = lines.Any(x => x == StringConstants.DescriptionCorrupted);

                var rarity = lines[0].Replace(StringConstants.DescriptionRarity, string.Empty);

                switch (rarity)
                {
                    case StringConstants.RarityUnique:
                        item = new EquippableItem();

                        if (isIdentitied)
                        {
                            item.Name = lines[1];
                            item.Type = lines[2];
                        }
                        else
                        {
                            item.Type = lines[1];
                        }

                        var links = GetLinkCount(lines.Where(c => c.StartsWith(StringConstants.DescriptionSockets)).FirstOrDefault());

                        if(links >= 5)
                        {
                            ((EquippableItem)item).Links = new SocketFilterOption()
                            {
                                Min = links,
                                Max = links,
                            };
                        }

                        ((EquippableItem)item).Rarity = rarity;
                        break;
                    case StringConstants.RarityRare:
                        item = new EquippableItem()
                        {
                            Name = lines[2].Replace(StringConstants.PrefixSuperior, string.Empty),
                            Type = lines[2].Replace(StringConstants.PrefixSuperior, string.Empty),
                            ItemLevel = GetNumberFromString(lines.Where(c => c.StartsWith(StringConstants.DescriptionItemLevel)).FirstOrDefault()),
                            Rarity = rarity,
                        };

                        var links2 = GetLinkCount(lines.Where(c => c.StartsWith(StringConstants.DescriptionSockets)).FirstOrDefault());     // better naming

                        if(links2 >= 5)
                        {
                            ((EquippableItem)item).Links = new SocketFilterOption()
                            {
                                Min = links2,
                                Max = links2,
                            };
                        }

                        var influence = GetInfluenceType(lines.LastOrDefault());

                        ((EquippableItem)item).Influence = influence;

                        break;
                    case StringConstants.RarityMagic:
                        break;
                    case StringConstants.RarityNormal:
                        if(lines.Any(c => c.StartsWith(StringConstants.DescriptionItemLevel)))      // Equippable Item
                        {
                            item = new EquippableItem()
                            {
                                Type = lines[1].Replace(StringConstants.PrefixSuperior, string.Empty),
                                ItemLevel = GetNumberFromString(lines.Where(c => c.StartsWith(StringConstants.DescriptionItemLevel)).FirstOrDefault()),
                                Rarity = rarity,                           // TODO Non-Unique Rarity
                            };

                            var influence2 = GetInfluenceType(lines.LastOrDefault());       // Better naming
                            ((EquippableItem)item).Influence = influence2;

                            var link3 = GetLinkCount(lines.Where(c => c.StartsWith(StringConstants.DescriptionSockets)).FirstOrDefault());      // Better naming

                            if(link3 >= 5)
                            {
                                ((EquippableItem)item).Links = new SocketFilterOption()
                                {
                                    Min = link3,
                                    Max = link3,
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
                        break;
                    case StringConstants.RarityCurrency:
                        item = new CurrencyItem()
                        {
                            Name = lines[1]
                        };
                        break;
                    case StringConstants.RarityGem:
                        item = new GemItem()
                        {
                            Type = lines[1],        // For Gems the Type has to be set to the Gem Name insead of the name itself
                            Level = GetNumberFromString(lines[4]),
                            Quality = hasQuality ? GetNumberFromString(lines.Where(x => x.StartsWith(StringConstants.DescriptionQuality)).FirstOrDefault()) : "0",      // Quality Line Can move for different Gems
                        };
                        break;
                    case StringConstants.RarityDivinationCard:
                        item = new CurrencyItem()
                        {
                            Name = lines[1],
                            Type = lines[1],
                        };
                        break;
                    default:
                        throw new NotImplementedException();
                }
            }
            catch (Exception e)
            {
                Logger.Log("Could not parse item. " + e.Message);
                Logger.Log("For now Sidekick only supports uniques.");
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
            if(input == null || !input.StartsWith(StringConstants.DescriptionSockets))
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
            else return 0;
        }

        internal static InfluenceType GetInfluenceType(string input)
        {
            switch(input)
            {
                case StringConstants.InfluenceShaper:
                    return InfluenceType.Shaper;
                case StringConstants.InfluenceElder:
                    return InfluenceType.Elder;
                case StringConstants.InfluenceCrusader:
                    return InfluenceType.Crusader;
                case StringConstants.InfluenceHunter:
                    return InfluenceType.Hunter;
                case StringConstants.InfluenceRedeemer:
                    return InfluenceType.Redeemer;
                case StringConstants.InfluenceWarlord:
                    return InfluenceType.Warlord;
                default:
                    return InfluenceType.None;
            }
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
