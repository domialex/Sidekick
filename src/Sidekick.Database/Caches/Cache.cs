using System.ComponentModel.DataAnnotations;

namespace Sidekick.Database.Caches
{
    public class Cache
    {
        [Key]
        public string Key { get; set; }

        public string Data { get; set; }
    }
}
