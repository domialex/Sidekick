using System.Collections.Generic;
using System.Text.Json.Serialization;
using Sidekick.Business.Apis.Poe.Trade.Data.Items;

namespace Sidekick.Business.Apis.Poe.Models
{
    public class Item
    {
        public string Name { get; set; }

        public string NameLine { get; set; }

        public string Type { get; set; }

        public string TypeLine { get; set; }

        public bool Identified { get; set; }

        [JsonPropertyName("ilvl")]
        public int ItemLevel { get; set; }

        [JsonPropertyName("frameType")]
        public Rarity Rarity { get; set; }

        [JsonIgnore]
        public Category Category { get; set; }

        public bool Corrupted { get; set; }

        public Properties Properties { get; set; } = new Properties();

        public Influences Influences { get; set; } = new Influences();

        public List<Socket> Sockets { get; set; } = new List<Socket>();

        public Modifiers Modifiers { get; set; } = new Modifiers();

        public string Text { get; set; }
    }
}
