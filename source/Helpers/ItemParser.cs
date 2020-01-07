using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sidekick.Helpers.POETradeAPI.Models;

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
                isIdentitied = !lines.Any(x => x == "Unidentified");
                hasQuality = lines.Any(x => x.Contains("Quality: "));
                isCorrupted = lines.Any(x => x == "Corrupted");

                var rarity = lines[0].Replace("Rarity: ", string.Empty);

                switch (rarity)
                {
                    case "Unique":
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
                        break;
                    case "Currency":
                        item = new CurrencyItem()
                        {
                            Name = lines[1]
                        };
                        break;
                    case "Gem":
                        item = new GemItem()
                        {
                            Type = lines[1],        // For Gems the Type has to be set to the Gem Name insead of the name itself
                            Level = GetNumberFromString(lines[4]),
                            Quality = hasQuality ? GetNumberFromString(lines.Where(x => x.StartsWith("Quality: ")).FirstOrDefault()) : "0",             // Quality Line Can move for different Gems
                        };
                        break;
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
}
