using Newtonsoft.Json;

namespace Sidekick.Business.Filters
{
    public class RequierementFilters
    {
        [JsonProperty(PropertyName = "lvl")]
        public FilterValue Level { get; set; }

        [JsonProperty(PropertyName = "dex")]
        public FilterValue Dexterity { get; set; }

        [JsonProperty(PropertyName = "str")]
        public FilterValue Strength { get; set; }

        [JsonProperty(PropertyName = "int")]
        public FilterValue Intelligence { get; set; }
    }
}
