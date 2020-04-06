using System.Collections.Generic;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Sidekick.Business.Apis.Poe.Trade.Search.Results
{
    public class Hashes
    {
        [JsonPropertyName("implicit")]
        public List<List<JsonElement>> ApiImplicit { get; set; }

        [JsonIgnore]
        public List<LineContentValue> Implicit => Parse(ApiImplicit);

        [JsonPropertyName("explicit")]
        public List<List<JsonElement>> ApiExplicit { get; set; }

        [JsonIgnore]
        public List<LineContentValue> Explicit => Parse(ApiExplicit);

        [JsonPropertyName("crafted")]
        public List<List<JsonElement>> ApiCrafted { get; set; }

        [JsonIgnore]
        public List<LineContentValue> Crafted => Parse(ApiCrafted);

        [JsonPropertyName("enchant")]
        public List<List<JsonElement>> ApiEnchant { get; set; }

        [JsonIgnore]
        public List<LineContentValue> Enchant => Parse(ApiEnchant);

        [JsonPropertyName("pseudo")]
        public List<List<JsonElement>> ApiPseudo { get; set; }

        [JsonIgnore]
        public List<LineContentValue> Pseudo => Parse(ApiPseudo);

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
