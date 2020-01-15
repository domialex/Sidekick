using Newtonsoft.Json;

namespace Sidekick.Business.Filters
{
    public class ArmorFilters
    {
        [JsonProperty(PropertyName = "ar")]
        public FilterValue Armor { get; set; }

        [JsonProperty(PropertyName = "es")]
        public FilterValue EnergyShield { get; set; }

        [JsonProperty(PropertyName = "ev")]
        public FilterValue Evasion { get; set; }

        [JsonProperty(PropertyName = "block")]
        public FilterValue Block { get; set; }
    }
}
