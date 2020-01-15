using Newtonsoft.Json;

namespace Sidekick.Business.Filters
{
    public class MapFilters
    {
        [JsonProperty(PropertyName = "map_iiq")]
        public FilterValue MapIiq { get; set; }

        [JsonProperty(PropertyName = "map_iir")]
        public FilterValue MapIir { get; set; }

        [JsonProperty(PropertyName = "map_tier")]
        public FilterValue MapTier { get; set; }

        [JsonProperty(PropertyName = "map_packsize")]
        public FilterValue MapPacksize { get; set; }

        [JsonProperty(PropertyName = "map_blighted")]
        public FilterOption Blighted { get; set; }

        [JsonProperty(PropertyName = "map_elder")]
        public FilterOption Elder { get; set; }

        [JsonProperty(PropertyName = "map_shaped")]
        public FilterOption Shaped { get; set; }
    }
}
