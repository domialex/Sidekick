using System.Text.Json.Serialization;

namespace Sidekick.Core.Update.Github_API
{
    public class GithubRelease
    {
        [JsonPropertyName("tag_name")]
        public string Tag { get; set; }
        public string Name { get; set; }
        public Asset[] Assets { get; set; }
    }
}
