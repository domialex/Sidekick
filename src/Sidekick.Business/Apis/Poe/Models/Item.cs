using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Sidekick.Business.Apis.Poe.Models
{
    public class Item
    {
        public List<Socket> Sockets { get; set; }

        public string Name { get; set; }

        public string TypeLine { get; set; }

        public bool Identified { get; set; }

        [JsonPropertyName("ilvl")]
        public int ItemLevel { get; set; }

        [JsonPropertyName("frameType")]
        public Rarity Rarity { get; set; }

        public bool Corrupted { get; set; }
    }
}
