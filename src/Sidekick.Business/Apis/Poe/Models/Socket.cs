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
                __Color = value switch
                {
                    SocketColor.Blue => "B",
                    SocketColor.Green => "G",
                    SocketColor.Red => "R",
                    SocketColor.White => "W",
                    SocketColor.Abyss => "A",
                    _ => throw new Exception("Invalid socket"),
                };
            }
        }
    }
}
