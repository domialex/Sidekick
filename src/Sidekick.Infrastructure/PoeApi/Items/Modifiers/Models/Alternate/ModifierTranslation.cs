using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Sidekick.Infrastructure.PoeApi.Items.Modifiers.Models.Alternate
{
    public class ModifierTranslation
    {
        [JsonPropertyName("English")]
        public List<Translation> Stats { get; set; }
    }
}
