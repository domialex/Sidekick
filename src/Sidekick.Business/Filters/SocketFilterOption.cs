using Newtonsoft.Json;

namespace Sidekick.Business.Filters
{
    public class SocketFilterOption : FilterValue
    {
        [JsonProperty(PropertyName = "r")]
        public int? Red { get; set; }

        [JsonProperty(PropertyName = "g")]
        public int? Green { get; set; }

        [JsonProperty(PropertyName = "b")]
        public int? Blue { get; set; }

        [JsonProperty(PropertyName = "w")]
        public int? White { get; set; }
    }
}
