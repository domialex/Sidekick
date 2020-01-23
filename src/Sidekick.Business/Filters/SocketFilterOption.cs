using System.Text.Json.Serialization;

namespace Sidekick.Business.Filters
{
    public class SocketFilterOption : FilterValue
    {
        [JsonPropertyName("r")]
        public int? Red { get; set; }

        [JsonPropertyName("g")]
        public int? Green { get; set; }

        [JsonPropertyName("b")]
        public int? Blue { get; set; }

        [JsonPropertyName("w")]
        public int? White { get; set; }
    }
}
