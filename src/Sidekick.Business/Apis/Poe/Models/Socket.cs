using System;
using System.Text.Json.Serialization;

namespace Sidekick.Business.Apis.Poe.Models
{
    public class Socket
    {
        public int Group { get; set; }

        [JsonPropertyName("sColour")]
        public string __Color { get; set; }

        [JsonIgnore]
        public SocketColor Color
        {
            get
            {
                return __Color switch
                {
                    "B" => SocketColor.Blue,
                    "G" => SocketColor.Green,
                    "R" => SocketColor.Red,
                    "W" => SocketColor.White,
                    "A" => SocketColor.Abyss,
                    _ => throw new Exception("Invalid socket"),
                };
            }
            set
            {
                switch (value)
                {
                    case SocketColor.Blue: __Color = "B"; break;
                    case SocketColor.Green: __Color = "G"; break;
                    case SocketColor.Red: __Color = "R"; break;
                    case SocketColor.White: __Color = "W"; break;
                    case SocketColor.Abyss: __Color = "A"; break;
                }
            }
        }
    }
}
