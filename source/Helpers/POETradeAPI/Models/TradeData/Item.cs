using System.Collections.Generic;

namespace Sidekick.Helpers.POETradeAPI.Models.TradeData
{
    /// <summary>
    /// Uniques
    /// Armour
    /// Cards
    /// Gems
    /// Jewels
    /// Maps
    /// Weapons
    /// Prophecies
    /// Itemised Monsters
    /// Watchstones
    /// </summary>
    public class ItemCategory
    {
        public string Label { get; set; }
        public List<Item> Entries { get; set; }
    }

    public class Item
    {
        public string Name { get; set; }
        public string Type { get; set; }
        public string Text { get; set; }
        public ItemFlags Flags { get; set; }
    }

    public class ItemFlags
    {
        public bool Prophecy { get; set; }
        public bool Unique { get; set; }
    }
}
