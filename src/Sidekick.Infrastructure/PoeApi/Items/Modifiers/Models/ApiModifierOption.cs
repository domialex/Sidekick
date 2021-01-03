using System.Text.Json;
using System.Text.Json.Serialization;

namespace Sidekick.Infrastructure.PoeApi.Items.Modifiers.Models
{
    public class ApiModifierOption
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
    }
}
