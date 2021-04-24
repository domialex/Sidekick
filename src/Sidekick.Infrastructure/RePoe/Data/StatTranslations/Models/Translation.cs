using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Sidekick.Infrastructure.RePoe.Data.StatTranslations.Models
{
    public class Translation
    {
        [JsonPropertyName("English")]
        public List<Stat> Stats { get; set; }
    }
}
