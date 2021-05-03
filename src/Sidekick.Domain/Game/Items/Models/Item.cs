using System.Collections.Generic;
using Sidekick.Domain.Game.Modifiers.Models;

namespace Sidekick.Domain.Game.Items.Models
{
    public class Item
    {
        public ItemMetadata Metadata { get; set; } = new();

        public OriginalItem Original { get; set; } = new();

        public Properties Properties { get; set; } = new();

        public Influences Influences { get; set; } = new();

        public List<Socket> Sockets { get; set; } = new();

        public ItemModifiers Modifiers { get; set; } = new();
    }
}
