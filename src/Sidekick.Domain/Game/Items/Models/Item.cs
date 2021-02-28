using System.Collections.Generic;
using Sidekick.Domain.Game.Items.Metadatas.Models;
using Sidekick.Domain.Game.Modifiers.Models;

namespace Sidekick.Domain.Game.Items.Models
{
    public class Item
    {
        public ItemMetadata Metadata { get; set; }

        public bool Identified { get; set; }

        public int ItemLevel { get; set; }

        public bool Corrupted { get; set; }

        public Properties Properties { get; set; } = new Properties();

        public Influences Influences { get; set; } = new Influences();

        public List<Socket> Sockets { get; set; } = new List<Socket>();

        public ItemModifiers Modifiers { get; set; } = new ItemModifiers();

        public ItemTexts Texts { get; set; } = new ItemTexts();
    }
}
