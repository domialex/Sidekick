using System.Collections.Generic;

namespace Sidekick.Business.Apis.Poe.Models
{
    public class Mods
    {
        public List<Mod> Implicit { get; set; } = new List<Mod>();
        public List<Mod> Explicit { get; set; } = new List<Mod>();
        public List<Mod> Crafted { get; set; } = new List<Mod>();
        public List<Mod> Enchant { get; set; } = new List<Mod>();
    }
}
