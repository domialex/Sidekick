using System.Text.Json.Serialization;
using System.Text.RegularExpressions;

namespace Sidekick.Business.Apis.Poe.Trade.Data.Stats
{
    public class StatData
    {
        public string Id { get; set; }
        public string Text { get; set; }
        public string Type { get; set; }

        public StatDataOption Option { get; set; }

        [JsonIgnore]
        public string Category { get; set; }

        [JsonIgnore]
        public Regex Pattern { get; set; }

        [JsonIgnore]
        public Regex NegativePattern { get; set; }

        [JsonIgnore]
        public Regex AdditionalProjectilePattern { get; set; }
    }
}
