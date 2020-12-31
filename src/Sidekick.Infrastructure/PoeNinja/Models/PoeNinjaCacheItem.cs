using System.Collections.Generic;

namespace Sidekick.Infrastructure.PoeNinja.Models
{
    public class PoeNinjaCacheItem<T>
    {
        public string Type { get; set; }

        public List<T> Items { get; set; }

        // public Dictionary<string, string> Translations { get; set; }
    }
}
