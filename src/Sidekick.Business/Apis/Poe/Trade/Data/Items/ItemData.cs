using System.Text.Json.Serialization;
using Sidekick.Business.Apis.Poe.Models;

namespace Sidekick.Business.Apis.Poe.Trade.Data.Items
{
    public class ItemData
    {
        public string Name { get; set; }
        public string Type { get; set; }
        public string Text { get; set; }
        public ItemDataFlags Flags { get; set; } = new ItemDataFlags();

        [JsonIgnore]
        public Rarity Rarity { get; set; }

        [JsonIgnore]
        public Category Category { get; set; }
    }
}
