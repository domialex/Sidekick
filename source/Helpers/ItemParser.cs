using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sidekick.Helpers.POETradeAPI.Models;
using System.Text.RegularExpressions;
using Sidekick.Helpers.Localization;
using Sidekick.Helpers.POETradeAPI;
using MoreLinq;

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
            if (!text.StartsWith(LanguageSettings.Provider.DescriptionRarity)) throw new Exception("Probably not an item.");

            try
            {
                var lines = text.Split(NEWLINE_SEPERATOR, StringSplitOptions.RemoveEmptyEntries).ToList();
                // If the item is Unidentified, the second line will be its Type instead of the Name.
                isIdentified = !lines.Any(x => x == LanguageSettings.Provider.DescriptionUnidentified);
                hasQuality = lines.Any(x => x.Contains(LanguageSettings.Provider.DescriptionQuality));
                isCorrupted = lines.Any(x => x == LanguageSettings.Provider.DescriptionCorrupted);
                isMap = lines.Any(x => x.Contains(LanguageSettings.Provider.DescriptionMapTier));
                isBlighted = lines.Any(x => x.Contains(LanguageSettings.Provider.PrefixBlighted));

                var rarity = lines[0].Replace(LanguageSettings.Provider.DescriptionRarity, string.Empty);

                if (isMap)
                {
                    item = new MapItem()
                    {
                        ItemQuantity = GetNumberFromString(lines.Where(c => c.StartsWith(LanguageSettings.Provider.DescriptionItemQuantity)).FirstOrDefault()),
                        ItemRarity = GetNumberFromString(lines.Where(c => c.StartsWith(LanguageSettings.Provider.DescriptionItemRarity)).FirstOrDefault()),
                        MonsterPackSize = GetNumberFromString(lines.Where(c => c.StartsWith(LanguageSettings.Provider.DescriptionMonsterPackSize)).FirstOrDefault()),
                        MapTier = GetNumberFromString(lines.Where(c => c.StartsWith(LanguageSettings.Provider.DescriptionMapTier)).FirstOrDefault()),
                        Rarity = rarity,
                    };

                    if (rarity == LanguageSettings.Provider.RarityNormal)
                    {
                        item.Type = lines[1].Replace(LanguageSettings.Provider.PrefixSuperior, string.Empty).Replace(LanguageSettings.Provider.PrefixBlighted, string.Empty).Trim();
                    }
                    else if (rarity == LanguageSettings.Provider.RarityMagic)        // Extract only map name
                    {
                        item.Type = TradeClient.MapNames.Where(c => lines[1].Contains(c)).FirstOrDefault();     // Search map name from statics
                    }
                    else if (rarity == LanguageSettings.Provider.RarityRare)
                    {
                        item.Type = lines[2].Replace(LanguageSettings.Provider.PrefixBlighted, string.Empty).Trim();
                    }
                    else if (rarity == LanguageSettings.Provider.RarityUnique)
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
                else if (rarity == LanguageSettings.Provider.RarityUnique)
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
                    ((EquippableItem)item).Stats = GetItemStats(text);
                }
                else if (rarity == LanguageSettings.Provider.RarityRare)
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
                    ((EquippableItem)item).Stats = GetItemStats(text);
                }
                else if (rarity == LanguageSettings.Provider.RarityMagic)
                {
                    throw new Exception("Magic items not supported for now.");
                    ((EquippableItem)item).Stats = GetItemStats(text);
                }
                else if (rarity == LanguageSettings.Provider.RarityNormal)
                {
                    if (lines.Any(c => c.StartsWith(LanguageSettings.Provider.DescriptionItemLevel)))      // Equippable Item
                    {
                        item = new EquippableItem()
                        {
                            Type = lines[1].Replace(LanguageSettings.Provider.PrefixSuperior, string.Empty).Trim(),
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
                else if (rarity == LanguageSettings.Provider.RarityCurrency)
                {
                    item = new CurrencyItem()
                    {
                        Name = lines[1]
                    };

                }
                else if (rarity == LanguageSettings.Provider.RarityGem)
                {
                    item = new GemItem()
                    {
                        Type = lines[1],        // For Gems the Type has to be set to the Gem Name insead of the name itself
                        Level = GetNumberFromString(lines[4]),
                        Quality = hasQuality ? GetNumberFromString(lines.Where(x => x.StartsWith(LanguageSettings.Provider.DescriptionQuality)).FirstOrDefault()) : "0",      // Quality Line Can move for different Gems
                    };
                }
                else if (rarity == LanguageSettings.Provider.RarityDivinationCard)
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
            if (string.IsNullOrEmpty(input))     // Default return 0
            {
                return "0";
            }

            return new string(input.Where(c => char.IsDigit(c)).ToArray());
        }

        internal static int GetLinkCount(string input)
        {
            var check = input.Replace(LanguageSettings.Provider.DescriptionSockets, string.Empty);
            return Regex.Replace(check, @"[RGBW]", string.Empty).Split(' ').MaxBy(x => x.Length).FirstOrDefault().Length + 1;
        }

        internal static InfluenceType GetInfluenceType(string input)
        {
            if (input.Contains(LanguageSettings.Provider.InfluenceShaper))
            {
                return InfluenceType.Shaper;
            }
            else if (input.Contains(LanguageSettings.Provider.InfluenceElder))
            {
                return InfluenceType.Elder;
            }
            else if (input.Contains(LanguageSettings.Provider.InfluenceCrusader))
            {
                return InfluenceType.Crusader;
            }
            else if (input.Contains(LanguageSettings.Provider.InfluenceHunter))
            {
                return InfluenceType.Hunter;
            }
            else if (input.Contains(LanguageSettings.Provider.InfluenceRedeemer))
            {
                return InfluenceType.Redeemer;
            }
            else if (input.Contains(LanguageSettings.Provider.InfluenceWarlord))
            {
                return InfluenceType.Warlord;
            }

            return InfluenceType.None;
        }

        internal static List<POETradeAPI.Models.TradeData.Attribute> GetItemStats(string text)
        {
            List<POETradeAPI.Models.TradeData.Attribute> stats = new List<POETradeAPI.Models.TradeData.Attribute>();

            var properties = text.Split(PROPERTY_SEPERATOR, StringSplitOptions.RemoveEmptyEntries).ToList();
            foreach (var prop in properties)
            {
                var lines = prop.Split(NEWLINE_SEPERATOR, StringSplitOptions.RemoveEmptyEntries).ToList();
                if (lines.FirstOrDefault().StartsWith(LanguageSettings.Provider.DescriptionRarity) ||
                    lines.FirstOrDefault().StartsWith(LanguageSettings.Provider.DescriptionSockets) ||
                    lines.FirstOrDefault().StartsWith(LanguageSettings.Provider.DescriptionQuality) ||
                    lines.FirstOrDefault().StartsWith(LanguageSettings.Provider.DescriptionMapTier) ||
                    lines.FirstOrDefault().StartsWith(LanguageSettings.Provider.DescriptionItemLevel) ||
                    lines.FirstOrDefault().StartsWith(LanguageSettings.Provider.DescriptionRequirements))
                {
                    continue;
                }

                //need LanguageSettings.Provider

                foreach (var line in lines)
                {
                    var search = Regex.Replace(line, @"([\+\-\d.]+)", "#");
                    if (search.EndsWith(" (implicit)"))
                    {
                        search = search.Replace(" (implicit)", string.Empty);
                        var category = TradeClient.AttributeCategories.FirstOrDefault(x => x.Label.Contains("Implicit"));
                        var res = category?.Entries.FirstOrDefault(x => x.Text == search);
                        if (res != null)
                            stats.Add(res);
                    }
                    else if (search.EndsWith(" (crafted)"))
                    {
                        search = search.Replace(" (crafted)", string.Empty);
                        var category = TradeClient.AttributeCategories.FirstOrDefault(x => x.Label.Contains("Crafted"));
                        var res = category?.Entries.FirstOrDefault(x => x.Text == search);
                        if (res != null)
                            stats.Add(res);
                    }
                    else if (search.EndsWith(" (fractured)"))
                    {
                        search = search.Replace(" (fractured)", string.Empty);
                        var category = TradeClient.AttributeCategories.FirstOrDefault(x => x.Label.Contains("Fractured"));
                        var res = category?.Entries.FirstOrDefault(x => x.Text == search);
                        if (res != null)
                            stats.Add(res);
                    }
                    else
                    {
                        //Find Properties
                        foreach (var cat in TradeClient.AttributeCategories.Where(x => x.Label == "Pseudo" || x.Label == "Explicit" || x.Label == "Enchant" || x.Label == "Monster" || x.Label == "Delve" || x.Label == "Veiled"))
                        {
                            var res = cat.Entries.FirstOrDefault(x => x.Text.StartsWith(search));
                            if (res != null)
                            {
                                stats.Add(res);
                            }
                        }
                    }
                }
            }

            return stats;
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

        private List<POETradeAPI.Models.TradeData.Attribute> _Stats;
        public List<POETradeAPI.Models.TradeData.Attribute> Stats
        {
            get
            {
                if (_Stats == null)
                {
                    _Stats = new List<POETradeAPI.Models.TradeData.Attribute>();
                }
                return _Stats;
            }
            set { _Stats = value; }
        }
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

    public class DivinationCardItem : Item
    {
    }

    public class FragmentItem : Item
    {
    }

    public class MapItem : Item
    {
        public string MapTier { get; set; }
        public string ItemQuantity { get; set; }
        public string ItemRarity { get; set; }
        public string MonsterPackSize { get; set; }
        public string Rarity { get; set; }
        public string IsBlight { get; set; }
    }
}
