using System.Text.Json.Serialization;

namespace Sidekick.Business.Filters
{
    public class RequierementFilters
    {
        [JsonPropertyName("lvl")]
        public FilterValue Level { get; set; }

        [JsonPropertyName("dex")]
        public FilterValue Dexterity { get; set; }

        [JsonPropertyName("str")]
        public FilterValue Strength { get; set; }

        [JsonPropertyName("int")]
        public FilterValue Intelligence { get; set; }
    }
}
