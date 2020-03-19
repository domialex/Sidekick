using System.Text.Json.Serialization;

namespace Sidekick.Business.Apis.Poe.Trade.Search.Filters
{
    public class RequirementFilter
    {
        [JsonPropertyName("lvl")]
        public SearchFilterValue Level { get; set; }

        [JsonPropertyName("dex")]
        public SearchFilterValue Dexterity { get; set; }

        [JsonPropertyName("str")]
        public SearchFilterValue Strength { get; set; }

        [JsonPropertyName("int")]
        public SearchFilterValue Intelligence { get; set; }
    }
}
