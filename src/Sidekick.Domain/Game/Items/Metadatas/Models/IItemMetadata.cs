using Sidekick.Domain.Game.Items.Models;

namespace Sidekick.Domain.Game.Items.Metadatas.Models
{
    public interface IItemMetadata
    {
        string Name { get; set; }

        string Type { get; set; }

        Rarity Rarity { get; set; }

        Category Category { get; set; }
    }
}
