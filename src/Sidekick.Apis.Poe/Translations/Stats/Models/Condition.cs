using System.Text.Json.Serialization;

namespace Sidekick.Apis.Poe.Translations.Stats.Models
{
    public class Condition
    {
        [JsonPropertyName("min")]
        public int? Min { get; set; }

        [JsonPropertyName("max")]
        public int? Max { get; set; }

        [JsonPropertyName("negated")]
        public bool? Negated { get; set; }

        public override string ToString()
        {
            var output = "";

            if (Min != null)
                output += $"Min: {Min} ";

            if (Max != null)
                output += $"Max: {Max}";

            return output != "" ? output : null;
        }
    }
}
