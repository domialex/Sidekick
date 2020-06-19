using System.ComponentModel.DataAnnotations;

namespace Sidekick.Database.ItemCategories
{
    public class ItemCategory
    {
        [Key]
        public string Type { get; set; }

        public string Category { get; set; }
    }
}
