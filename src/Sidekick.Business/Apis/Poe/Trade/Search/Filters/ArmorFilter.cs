using System.Text.Json.Serialization;

namespace Sidekick.Business.Apis.Poe.Trade.Search.Filters
{
    public class ArmorFilter
    {
        [JsonPropertyName("ar")]
        public FilterValue Armor { get; set; }

        [JsonPropertyName("es")]
        public FilterValue EnergyShield { get; set; }

        [JsonPropertyName("ev")]
        public FilterValue Evasion { get; set; }

        [JsonPropertyName("block")]
        public FilterValue Block { get; set; }
    }
}
