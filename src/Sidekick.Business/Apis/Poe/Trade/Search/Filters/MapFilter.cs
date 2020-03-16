using System.Text.Json.Serialization;

namespace Sidekick.Business.Apis.Poe.Trade.Search.Filters
{
    public class MapFilter
    {
        [JsonPropertyName("map_iiq")]
        public FilterValue MapIiq { get; set; }

        [JsonPropertyName("map_iir")]
        public FilterValue MapIir { get; set; }

        [JsonPropertyName("map_tier")]
        public FilterValue MapTier { get; set; }

        [JsonPropertyName("map_packsize")]
        public FilterValue MapPacksize { get; set; }

        [JsonPropertyName("map_blighted")]
        public FilterOption Blighted { get; set; }

        [JsonPropertyName("map_elder")]
        public FilterOption Elder { get; set; }

        [JsonPropertyName("map_shaped")]
        public FilterOption Shaped { get; set; }
    }
}
