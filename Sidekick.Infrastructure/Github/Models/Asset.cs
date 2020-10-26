using System.Text.Json.Serialization;

namespace Sidekick.Infrastructure.Github.Models
{
    public class Asset
    {
        public string Url { get; set; }
        public string Name { get; set; }
        [JsonPropertyName("browser_download_url")]
        public string DownloadUrl { get; set; }
    }
}
