using System.Collections.Generic;
using Sidekick.Domain.Game.Items.Metadatas.Models;
using Sidekick.Domain.Game.Modifiers.Models;

namespace Sidekick.Domain.Game.Items.Models
{
    public class Item : IItemMetadata
    {
        public string Name { get; set; }

        public string NameLine { get; set; }

        public string Type { get; set; }

        public string TypeLine { get; set; }

        public bool Identified { get; set; }

        public int ItemLevel { get; set; }

        public Rarity Rarity { get; set; }

        public Category Category { get; set; }

        public bool Corrupted { get; set; }

        public Properties Properties { get; set; } = new Properties();

        public Influences Influences { get; set; } = new Influences();

        public List<Socket> Sockets { get; set; } = new List<Socket>();

        public ItemModifiers Modifiers { get; set; } = new ItemModifiers();

        public string Text { get; set; }
    }
}
