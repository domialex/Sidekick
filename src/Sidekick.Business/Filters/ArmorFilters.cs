using System.Text.Json.Serialization;

namespace Sidekick.Business.Filters
{
    public class ArmorFilters
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
