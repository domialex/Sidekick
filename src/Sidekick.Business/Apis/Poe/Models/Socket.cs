using System;
using System.Text.Json.Serialization;

namespace Sidekick.Business.Apis.Poe.Models
{
    public class Socket
    {
        public int Group { get; set; }

        [JsonPropertyName("sColour")]
        public string ColourString { get; set; }

        [JsonIgnore]
        public SocketColour Colour
        {
            get
            {
                return ColourString switch
                {
                    "B" => SocketColour.Blue,
                    "G" => SocketColour.Green,
                    "R" => SocketColour.Red,
                    "W" => SocketColour.White,
                    "A" => SocketColour.Abyss,
                    _ => throw new Exception("Invalid socket"),
                };
            }
            set
            {
                ColourString = value switch
                {
                    SocketColour.Blue => "B",
                    SocketColour.Green => "G",
                    SocketColour.Red => "R",
                    SocketColour.White => "W",
                    SocketColour.Abyss => "A",
                    _ => throw new Exception("Invalid socket"),
                };
            }
        }
    }
}
