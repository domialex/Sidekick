using System.Text.Json.Serialization;

namespace Sidekick.Infrastructure.PoeApi.Trade.Results
{
    public class ResultSocket
  {
    public int Group { get; set; }

    [JsonPropertyName("sColour")]
    public string ColourString { get; set; }
  }
}
