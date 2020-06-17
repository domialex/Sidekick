using System.Text.Json;
using System.Text.Json.Serialization;
using System.Text.RegularExpressions;

namespace Sidekick.Business.Apis.Poe.Models
{
    public class ModifierOption
    {
        private JsonElement jsonId;
        // Needs to be object because some Ids are strings, and some are ints
        [JsonPropertyName("id")]
        public JsonElement JsonId
        {
            get
            {
                return jsonId;
            }
            set
            {
                jsonId = value;

                if (jsonId.ValueKind == JsonValueKind.Number && jsonId.TryGetInt32(out var intValue))
                {
                    Value = intValue;
                }
                else
                {
                    Value = jsonId.GetString();
                }
            }
        }

        public object Value { get; set; }

        public string Text { get; set; }

        [JsonIgnore]
        public Regex Pattern { get; set; }
    }
}
