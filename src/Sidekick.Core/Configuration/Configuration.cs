using System.IO;
using System.Text.Json;

namespace Sidekick.Core.Configuration
{
    public class Configuration
    {
        public const string FileName = "appsettings.json";

        public string LeagueId { get; set; }

        public void Save()
        {
            var json = JsonSerializer.Serialize(this);

            var filePath = Path.Combine(Directory.GetCurrentDirectory(), FileName);
            var jsonString = File.ReadAllText(filePath);

            using (var fileStream = File.Create(filePath))
            {
                using (var writer = new Utf8JsonWriter(fileStream, options: new JsonWriterOptions
                {
                    Indented = true
                }))
                {
                    using (var document = JsonDocument.Parse(json, new JsonDocumentOptions
                    {
                        CommentHandling = JsonCommentHandling.Skip
                    }))
                    {
                        JsonElement root = document.RootElement;

                        if (root.ValueKind == JsonValueKind.Object)
                        {
                            writer.WriteStartObject();
                        }
                        else
                        {
                            return;
                        }

                        foreach (JsonProperty property in root.EnumerateObject())
                        {
                            property.WriteTo(writer);
                        }

                        writer.WriteEndObject();
                        writer.Flush();
                    }
                }
            }
        }
    }
}
