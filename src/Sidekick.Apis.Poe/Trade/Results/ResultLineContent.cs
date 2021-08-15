using System.Collections.Generic;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Sidekick.Apis.Poe.Trade.Results
{
    public class ResultLineContent
    {
        public string Name { get; set; }

        [JsonPropertyName("values")]
        public List<List<JsonElement>> Values { get; set; }

        public int DisplayMode { get; set; }

        [JsonPropertyName("type")]
        public int Order { get; set; }
    }
}
