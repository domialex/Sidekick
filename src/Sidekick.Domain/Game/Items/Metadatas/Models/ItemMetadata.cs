using Sidekick.Domain.Game.Items.Models;

namespace Sidekick.Domain.Game.Items.Metadatas.Models
{
    public class ItemMetadata
    {
        public string Name { get; set; }

        public string Type { get; set; }

        public Rarity Rarity { get; set; }

        public Category Category { get; set; }
    }
}
