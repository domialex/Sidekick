using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Sidekick.Apis.Poe.Translations.Stats.Models
{
    public class Translation
    {
        [JsonPropertyName("English")]
        public List<Stat> Stats { get; set; }
    }
}
