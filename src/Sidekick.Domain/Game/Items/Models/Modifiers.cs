using System.Collections.Generic;

namespace Sidekick.Domain.Game.Items.Models
{
    public class Modifiers
    {
        public List<Modifier> Implicit { get; set; } = new List<Modifier>();
        public List<Modifier> Explicit { get; set; } = new List<Modifier>();
        public List<Modifier> Crafted { get; set; } = new List<Modifier>();
        public List<Modifier> Enchant { get; set; } = new List<Modifier>();
        public List<Modifier> Pseudo { get; set; } = new List<Modifier>();
        public List<Modifier> Fractured { get; set; } = new List<Modifier>();
    }
}
