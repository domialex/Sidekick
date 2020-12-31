using System.Text.Json.Serialization;

namespace Sidekick.Infrastructure.PoeApi.Trade.Filters
{
    public class MapFilter
    {
        [JsonPropertyName("map_iiq")]
        public SearchFilterValue ItemQuantity { get; set; }

        [JsonPropertyName("map_iir")]
        public SearchFilterValue ItemRarity { get; set; }

        [JsonPropertyName("map_tier")]
        public SearchFilterValue MapTier { get; set; }

        [JsonPropertyName("map_packsize")]
        public SearchFilterValue MonsterPackSize { get; set; }

        [JsonPropertyName("map_blighted")]
        public SearchFilterOption Blighted { get; set; }

        [JsonPropertyName("map_elder")]
        public SearchFilterOption Elder { get; set; }

        [JsonPropertyName("map_shaped")]
        public SearchFilterOption Shaped { get; set; }
    }
}
