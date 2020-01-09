using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sidekick.Helpers.POETradeAPI.Models;
using System.Text.RegularExpressions;
using MoreLinq;
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
            bool isIdentified, hasQuality, isCorrupted;

            try
            {
                // Splitting Properties
                var properties = text.Split(PROPERTY_SEPERATOR, StringSplitOptions.RemoveEmptyEntries).ToList();
                foreach (var prop in properties)
                {
                    var lines = prop.Split(NEWLINE_SEPERATOR, StringSplitOptions.RemoveEmptyEntries);

                    if (lines.FirstOrDefault().StartsWith(StringConstants.DescriptionRarity))
                    {
                        item = new EquippableItem();
                        ((EquippableItem)item).Rarity = lines.FirstOrDefault().Replace(StringConstants.DescriptionRarity, string.Empty);
                        item.Name = lines.Skip(1).FirstOrDefault();
                        item.Type = lines.LastOrDefault();
                        item.Type = item.Type == item.Name ? item.Type.Replace("Superior ", string.Empty) : item.Type;
                    }
                    else if (lines.FirstOrDefault().StartsWith(StringConstants.DescriptionRequirements))
                    {
                        ((EquippableItem)item).Req = new ReqValue();
                        foreach (var line in lines)
                        {
                            if (line.StartsWith("Level: "))
                            {
                                int.TryParse(line.Replace("Level: ", string.Empty), out int res);
                                ((EquippableItem)item).Req.Level = res;
                            }
                            else if (line.StartsWith("Dex: "))
                            {
                                int.TryParse(line.Replace("Dex: ", string.Empty), out int res);
                                ((EquippableItem)item).Req.Dex = res;
                            }
                            else if (line.StartsWith("Str: "))
                            {
                                int.TryParse(line.Replace("Str: ", string.Empty), out int res);
                                ((EquippableItem)item).Req.Str = res;
                            }
                            else if (line.StartsWith("Int: "))
                            {
                                int.TryParse(line.Replace("Int: ", string.Empty), out int res);
                                ((EquippableItem)item).Req.Int = res;
                            }
                        }
                    }
                    else if (lines.FirstOrDefault().Contains(StringConstants.DescriptionSockets))
                    {
                        var socklinks = lines.FirstOrDefault().Replace(StringConstants.DescriptionSockets, string.Empty);
                        ((EquippableItem)item).Socket = new SocketValue
                        {
                            R = socklinks.Count(x => x == 'R'),
                            G = socklinks.Count(x => x == 'G'),
                            B = socklinks.Count(x => x == 'B'),
                            W = socklinks.Count(x => x == 'W')
                        };
                        ((EquippableItem)item).Socket.Sockets = ((EquippableItem)item).Socket.R + ((EquippableItem)item).Socket.G + ((EquippableItem)item).Socket.B + ((EquippableItem)item).Socket.W;
                        ((EquippableItem)item).Socket.Links = Regex.Replace(socklinks, @"[RGBW]", string.Empty).Split(' ').MaxBy(x => x.Length).FirstOrDefault().Length + 1;
                    }
                    else if (lines.FirstOrDefault().StartsWith(StringConstants.DescriptionItemLevel))
                    {
                        int.TryParse(lines.FirstOrDefault().Replace(StringConstants.DescriptionItemLevel, string.Empty), out int res);
                        ((EquippableItem)item).ItemLevel = res;
                    }
                    else if (lines.FirstOrDefault().StartsWith(StringConstants.DescriptionQuality) || lines.FirstOrDefault().StartsWith(StringConstants.DescriptionArmour) || lines.FirstOrDefault().StartsWith(StringConstants.DescriptionEvasion) || lines.FirstOrDefault().StartsWith(StringConstants.DescriptionEnergyShield) || lines.FirstOrDefault().StartsWith(StringConstants.DescriptionBlock))
                    {
                        ((EquippableItem)item).Armour = new ArmourValue();
                        foreach (var line in lines)
                        {
                            var match = Regex.Match(line, @"[\d]+");
                            int number = 0;
                            if (match.Success)
                                int.TryParse(match.Value, out number);

                            if (line.StartsWith("Quality: "))
                            {
                                ((EquippableItem)item).Armour.Quality = number;
                            }
                            else if (line.StartsWith("Armour: "))
                            {
                                ((EquippableItem)item).Armour.Armour = number;
                            }
                            else if (line.StartsWith("Evasion: "))
                            {
                                ((EquippableItem)item).Armour.Evasion = number;
                            }
                            else if (line.StartsWith("Energy Shield: "))
                            {
                                ((EquippableItem)item).Armour.EnergyShield = number;
                            }
                            else if (line.StartsWith("Block: "))
                            {
                                ((EquippableItem)item).Armour.Block = number;
                            }
                        }
                    }
                    else if (lines.FirstOrDefault().StartsWith(StringConstants.DescriptionMapTier))
                    {
                        int.TryParse(lines.FirstOrDefault().Replace(StringConstants.DescriptionMapTier, string.Empty), out int tier);
                        ((EquippableItem)item).MapTier = tier;
                    }
                    else if (lines.FirstOrDefault().EndsWith(" Item"))
                    {
                        Enum.TryParse(lines.FirstOrDefault().Replace(" Item", string.Empty), out InfluenceType result);
                        ((EquippableItem)item).Influence = result;
                    }
                    else if (lines.Any(x => x == "Unidentified"))
                    {
                        //item.IsIdentified = true;
                    }
                    else
                    {
                        foreach (var line in lines)
                        {
                            var search = Regex.Replace(line, @"([\+\-\d.]+)", "#");
                            if (search.EndsWith(" (implicit)"))
                            {
                                search = search.Replace(" (implicit)", string.Empty);
                                //To Do relace The Elder,... with #
                                var category = TradeClient.AttributeCategories.FirstOrDefault(x => x.Label.Contains("Implicit"));
                                var res = category?.Entries.FirstOrDefault(x => x.Text == search);
                                if (res != null)
                                    ((EquippableItem)item).Stats.Add(res);
                            }
                            else if (search.EndsWith(" (crafted)"))
                            {
                                search = search.Replace(" (crafted)", string.Empty);
                                var category = TradeClient.AttributeCategories.FirstOrDefault(x => x.Label.Contains("Crafted"));
                                var res = category?.Entries.FirstOrDefault(x => x.Text == search);
                                if (res != null)
                                    ((EquippableItem)item).Stats.Add(res);
                            }
                            else if (search.EndsWith(" (fractured)"))
                            {
                                search = search.Replace(" (fractured)", string.Empty);
                                var category = TradeClient.AttributeCategories.FirstOrDefault(x => x.Label.Contains("Fractured"));
                                var res = category?.Entries.FirstOrDefault(x => x.Text == search);
                                if (res != null)
                                    ((EquippableItem)item).Stats.Add(res);
                            }
                            else if (search.EndsWith(" (veiled)"))
                            {
                                search = search.Replace(" (veiled)", string.Empty);
                                var category = TradeClient.AttributeCategories.FirstOrDefault(x => x.Label.Contains("Veiled"));
                                var res = category?.Entries.FirstOrDefault(x => x.Text == search);
                                if (res != null)
                                    ((EquippableItem)item).Stats.Add(res);
                            }
                            else if (search.EndsWith(" (delve)"))
                            {
                                search = search.Replace(" (delve)", string.Empty);
                                var category = TradeClient.AttributeCategories.FirstOrDefault(x => x.Label.Contains("Delve"));
                                var res = category?.Entries.FirstOrDefault(x => x.Text == search);
                                if (res != null)
                                    ((EquippableItem)item).Stats.Add(res);
                            }
                            else
                            {
                                //Find Properties
                                foreach (var cat in TradeClient.AttributeCategories.Where(x => x.Label == "Pseudo" || x.Label == "Explicit" || x.Label == "Enchant" || x.Label == "Monster"))
                                {
                                    //StartsWith for (local)
                                    var res = cat.Entries.FirstOrDefault(x => x.Text.StartsWith(search));
                                    if (res != null)
                                    {
                                        ((EquippableItem)item).Stats.Add(res);
                                    }
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception e)
            {
                Logger.Log("Could not parse item. " + e.Message);
                return null;
            }

            return item;
        }

        //Using MoreLinq instead
        internal static string GetNumberFromString(string input)
        {
            return new string(input.Where(c => char.IsDigit(c)).ToArray());
        }
        //Using MoreLinq instead
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

        //Using Enum.TryParse instead
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
        public int ItemLevel { get; set; }
        public InfluenceType Influence { get; set; }
        public SocketFilterOption Sockets { get; set; }
        public SocketFilterOption Links { get; set; }
        public int MapTier { get; set; }
        public ReqValue Req { get; set; }
        public SocketValue Socket { get; set; }
        public ArmourValue Armour { get; set; }

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

    public class FragmentItem : Item
    {
    }




    public class ReqValue
    {
        public int Level { get; set; }
        public int Str { get; set; }
        public int Dex { get; set; }
        public int Int { get; set; }
    }

    public class SocketValue
    {
        public int Sockets { get; set; }
        public int Links { get; set; }
        public int R { get; set; }
        public int G { get; set; }
        public int B { get; set; }
        public int W { get; set; }
    }

    public class ArmourValue
    {
        public int Quality { get; set; }
        public int Armour { get; set; }
        public int Evasion { get; set; }
        public int EnergyShield { get; set; }
        public int Block { get; set; }
    }

}
