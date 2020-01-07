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
            var item = new Item();
            bool isIdentitied;

            try
            {
                var lines = text.Split(NEWLINE_SEPERATOR, StringSplitOptions.RemoveEmptyEntries);
                // Every item should start with Rarity in the first line.
                if (!lines[0].StartsWith("Rarity: ")) throw new Exception("Probably not an item.");
                // If the item is Unidentified, the second line will be its Type instead of the Name.
                isIdentitied = !lines.Any(x => x == "Unidentified");

                item.Rarity = lines[0].Replace("Rarity: ", string.Empty);

                switch (item.Rarity)
                {
                    case "Unique":
                    //case "Rare":
                        if (isIdentitied)
                        {
                            item.Name = lines[1];
                            item.Type = lines[2];
                            item.Links = GetLinks(lines.FirstOrDefault(qq => qq.StartsWith("Sockets: ")));
                        }
                        else
                        {
                            item.Type = lines[1];
                        }
                        break;
                    case "Currency":
                        item.Name = lines[1];
                        break;
                }
            }
            catch(Exception e)
            {
                Logger.Log("Could not parse item. " + e.Message);
                Logger.Log("For now Sidekick only supports uniques.");
                return null;
            }

            return item;
        }
        private static int GetLinks(string line)
        {
            if (!String.IsNullOrEmpty(line))
            {
                var regex = new Regex("[a-zA-Z]+[-]");
                return regex.Matches(line).Count + 1;
            }
            else return 0;
        }
    }

    public class Item
    {
        public string Name { get; set; }
        public string Type { get; set; }
        public string Rarity { get; set; }
        public int Links { get; set; }
    }
}
