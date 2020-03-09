using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Text.RegularExpressions;

namespace Sidekick.Business.Trades.Results
{
    public class LineContent
    {
        public string Name { get; set; }

        [JsonPropertyName("values")]
        public List<List<JsonElement>> __Values { get; set; }

        public int DisplayMode { get; set; }

        [JsonPropertyName("type")]
        public int Order { get; set; }

        private List<LineContentValue> values = null;
        [JsonIgnore]
        public List<LineContentValue> Values
        {
            get
            {
                if (values != null)
                {
                    return values;
                }

                var result = new List<LineContentValue>();
                foreach (var value in __Values)
                {
                    if (value.Count != 2)
                    {
                        continue;
                    }

                    var stringValue = (JsonElement)value[0];
                    var type = (JsonElement)value[1];

                    result.Add(new LineContentValue()
                    {
                        Value = stringValue.GetString(),
                        Type = (LineContentType)type.GetInt32()
                    });
                }
                return result;
            }
            set
            {
                values = value;
            }
        }

        [JsonIgnore]
        public string Parsed
        {
            get
            {
                if (Values.Count == 0)
                {
                    return Name;
                }

                switch (DisplayMode)
                {
                    case 0:
                        return $"{Name}: {string.Join(", ", Values.Select(x => x.Value))}";
                    case 1:
                        return $"{Values[0].Value} {Name}";
                    case 2:
                        return $"{Values[0].Value}";
                    case 3:
                        var format = Regex.Replace(Name, "%(\\d)", "{$1}");
                        return string.Format(format, Values.Select(x => x.Value).ToArray());
                    default:
                        return Name;
                }
            }
        }
    }
}
