using System.Collections.Generic;
using Sidekick.Domain.Game.Items.Metadatas.Models;
using Sidekick.Domain.Game.Modifiers.Models;

namespace Sidekick.Domain.Game.Items.Models
{
    public class Item
    {
        public ItemMetadata Metadata { get; set; }

        public OriginalItem Original { get; set; }

        public Properties Properties { get; set; }

        public Influences Influences { get; set; }

        public List<Socket> Sockets { get; set; }

        public ItemModifiers Modifiers { get; set; }
    }
}
