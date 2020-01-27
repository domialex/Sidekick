using System.IO;
using System.Text.Json;

namespace Sidekick.Core.Settings
{
    public class SidekickSettings
    {
        public const string FileName = "Sidekick_settings.json";

        public string UILanguage { get; set; }

        public string LeagueId { get; set; }

        public WikiSetting CurrentWikiSettings { get; set; }

        public string CharacterName { get; set; }

        public bool RetainClipboard { get; set; }
        public bool CloseOverlayWithMouse { get; set; }
        public bool EnableCtrlScroll { get; set; }

        public string KeyCloseWindow { get; set; }

        public string KeyPriceCheck { get; set; }

        public string KeyHideout { get; set; }

        public string KeyItemWiki { get; set; }

        public string KeyFindItems { get; set; }

        public string KeyLeaveParty { get; set; }

        public string KeyOpenSearch { get; set; }

        public string KeyOpenLeagueOverview { get; set; }

        public void Save()
        {
            var json = JsonSerializer.Serialize(this);
            var defaults = JsonSerializer.Serialize(DefaultSettings.Settings);
            var filePath = Path.Combine(Directory.GetCurrentDirectory(), FileName);

            // Backup old settings
            if (File.Exists(filePath))
            {
                File.Copy(filePath, filePath.Replace(".json", "_old.json"), true);
            }

            // TODO: Refactor this to use the new using syntax in Csharp 8
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
                        using (var defaultsDocument = JsonDocument.Parse(defaults, new JsonDocumentOptions
                        {
                            CommentHandling = JsonCommentHandling.Skip
                        }))
                        {
                            var root = document.RootElement;
                            var defaultsRoot = defaultsDocument.RootElement;

                            if (root.ValueKind == JsonValueKind.Object)
                            {
                                writer.WriteStartObject();
                            }
                            else
                            {
                                return;
                            }

                            foreach (var property in root.EnumerateObject())
                            {
                                if (defaultsRoot.GetProperty(property.Name).ToString() == property.Value.ToString())
                                {
                                    continue;
                                }

                                property.WriteTo(writer);
                            }

                            writer.WriteEndObject();
                            writer.Flush();
                        }
                    }

                    if (writer.BytesCommitted == 0)
                    {
                        File.Delete(filePath);
                    }
                }
            }
        }
    }
}
