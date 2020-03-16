using System.Text.Json.Serialization;

namespace Sidekick.Business.Apis.Poe.Trade.Search.Filters
{
    public class RequirementFilter
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
