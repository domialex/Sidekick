using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace Sidekick.Domain.Game.StatTranslations
{
    public class StatTranslation
    {
        [JsonPropertyName("English")]
        public List<Stat> Stats { get; set; }
    }

    public class Stat
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

    public class Condition
    {
        [JsonPropertyName("min")]
        public int? Min { get; set; }

        [JsonPropertyName("max")]
        public int? Max { get; set; }

        [JsonPropertyName("negated")]
        public bool? Negated { get; set; }

        public override string ToString()
        {
            string output = "";

            if (Min != null)
                output += $"Min: {Min} ";

            if (Max != null)
                output += $"Max: {Max}";

            return output != "" ? output : null;
        }
    }
}
