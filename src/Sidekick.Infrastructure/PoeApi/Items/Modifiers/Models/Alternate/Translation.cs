using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Sidekick.Infrastructure.PoeApi.Items.Modifiers.Models.Alternate
{
    public class Translation
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
