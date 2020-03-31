using System.Text.Json.Serialization;

namespace Sidekick.Business.Apis.Poe.Models
{
    public class Extended
    {
        public string Text { get; set; }

        public Mods Mods { get; set; } = new Mods();

        [JsonPropertyName("dps")]
        public double DamagePerSecond { get; set; }

        [JsonPropertyName("edps")]
        public double ElementalDps { get; set; }

        [JsonPropertyName("pdps")]
        public double PhysicalDps { get; set; }
    }
}
