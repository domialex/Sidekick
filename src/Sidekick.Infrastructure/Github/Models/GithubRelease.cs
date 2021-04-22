using System.Text.Json.Serialization;

namespace Sidekick.Infrastructure.Github.Models
{
    public class GithubRelease
    {
        [JsonPropertyName("tag_name")]
        public string Tag { get; set; }
        public string Name { get; set; }
        public bool Prerelease { get; set; }
        public Asset[] Assets { get; set; }
    }
}
