using System.Collections.Generic;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Sidekick.Business.Apis.Poe.Models
{
    public class Hashes
    {
        [JsonPropertyName("implicit")]
        public List<List<JsonElement>> __Implicit { get; set; }

        [JsonIgnore]
        public List<LineContentValue> Implicit => Parse(__Implicit);

        [JsonPropertyName("explicit")]
        public List<List<JsonElement>> __Explicit { get; set; }

        [JsonIgnore]
        public List<LineContentValue> Explicit => Parse(__Explicit);

        [JsonPropertyName("crafted")]
        public List<List<JsonElement>> __Crafted { get; set; }

        [JsonIgnore]
        public List<LineContentValue> Crafted => Parse(__Crafted);

        [JsonPropertyName("enchant")]
        public List<List<JsonElement>> __Enchant { get; set; }

        [JsonIgnore]
        public List<LineContentValue> Enchant => Parse(__Enchant);

        [JsonPropertyName("pseudo")]
        public List<List<JsonElement>> __Pseudo { get; set; }

        [JsonIgnore]
        public List<LineContentValue> Pseudo => Parse(__Pseudo);

        private List<LineContentValue> Parse(List<List<JsonElement>> values)
        {
            var result = new List<LineContentValue>();
            if (values != null)
            {
                foreach (var value in values)
                {
                    if (value.Count != 2 || value[1].ValueKind != JsonValueKind.Array)
                    {
                        continue;
                    }

                    result.Add(new LineContentValue()
                    {
                        Value = value[0].GetString(),
                        Type = (LineContentType)value[1][0].GetInt32()
                    });
                }
            }
            return result;
        }
    }
}
