using System.Collections.Generic;

namespace Sidekick.Domain.Game.Modifiers.Models
{
    public class ItemModifiers
    {
        public List<Modifier> Implicit { get; set; } = new();
        public List<Modifier> Explicit { get; set; } = new();
        public List<Modifier> Crafted { get; set; } = new();
        public List<Modifier> Enchant { get; set; } = new();
        public List<Modifier> Pseudo { get; set; } = new();
        public List<Modifier> Fractured { get; set; } = new();
    }
}
