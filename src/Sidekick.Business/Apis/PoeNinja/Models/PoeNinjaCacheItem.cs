using System.Collections.Generic;

namespace Sidekick.Business.Apis.PoeNinja.Models
{
    public class PoeNinjaCacheItem<T> where T : PoeNinjaResult
    {
        public string Type { get; set; }

        public List<T> Items { get; set; }
    }
}
