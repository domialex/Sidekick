using System.Text.Json.Serialization;

namespace Sidekick.Business.Apis.Poe.Trade.Search.Filters
{
    public class ArmorFilter
    {
        [JsonPropertyName("ar")]
        public SearchFilterValue Armor { get; set; }

        [JsonPropertyName("es")]
        public SearchFilterValue EnergyShield { get; set; }

        [JsonPropertyName("ev")]
        public SearchFilterValue Evasion { get; set; }

        [JsonPropertyName("block")]
        public SearchFilterValue Block { get; set; }
    }
}
