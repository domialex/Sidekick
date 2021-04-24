using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Sidekick.Infrastructure.RePoe.Data.StatTranslations.Models
{
    public class Stat
    {
        [JsonPropertyName("condition")]
        public List<Condition> Conditions { get; set; }

        [JsonPropertyName("format")]
        public List<string> Formats { get; set; }

        [JsonPropertyName("index_handlers")]
        public List<List<string>> IndexHandlers { get; set; }

        [JsonPropertyName("string")]
        public string Text { get; set; }

        public override string ToString()
        {
            return Text;
        }
    }
}
