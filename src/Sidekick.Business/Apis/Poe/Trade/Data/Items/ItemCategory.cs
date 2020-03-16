using System.Collections.Generic;

namespace Sidekick.Business.Apis.Poe.Trade.Data.Items
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
}
