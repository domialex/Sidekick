using System.Collections.Generic;
using System.IO;
using System.Text.Json;

namespace Sidekick.Core.Settings
{
    public class SidekickSettings
    {
        public const string FileName = "Sidekick_settings.json";

        public string Language_UI { get; set; }

        public string Language_Parser { get; set; }

        public string LeagueId { get; set; }

        public string LeaguesHash { get; set; }

        public int League_SelectedTabIndex { get; set; }

        public WikiSetting Wiki_Preferred { get; set; }

        public string Character_Name { get; set; }

        public bool RetainClipboard { get; set; }

        public bool CloseOverlayWithMouse { get; set; }

        public bool EnableCtrlScroll { get; set; }

        public bool EnablePricePrediction { get; set; }

        public bool ShowSplashScreen { get; set; }

        public string DangerousModsRegex { get; set; }

        public List<string> AccessoryModifiers { get; set; }
        public List<string> ArmourModifiers { get; set; }
        public List<string> FlaskModifiers { get; set; }
        public List<string> JewelModifiers { get; set; }
        public List<string> MapModifiers { get; set; }
        public List<string> WeaponModifiers { get; set; }

        public string Key_CloseWindow { get; set; }

        public string Key_CheckPrices { get; set; }

        public string Key_MapInfo { get; set; }

        public string Key_GoToHideout { get; set; }

        public string Key_OpenWiki { get; set; }

        public string Key_FindItems { get; set; }

        public string Key_LeaveParty { get; set; }

        public string Key_OpenSearch { get; set; }

        public string Key_OpenLeagueOverview { get; set; }

        public string Key_ReplyToLatestWhisper { get; set; }

        public string Key_Exit { get; set; }

        public string Key_Stash_Left { get; set; }

        public string Key_Stash_Right { get; set; }

        public void Save()
        {
            var json = JsonSerializer.Serialize(this);
            var defaults = JsonSerializer.Serialize(DefaultSettings.Settings);
            var filePath = Path.Combine(Directory.GetCurrentDirectory(), FileName);

            using var fileStream = File.Create(filePath);
            using var writer = new Utf8JsonWriter(fileStream, options: new JsonWriterOptions
            {
                Indented = true
            });
            using var document = JsonDocument.Parse(json, new JsonDocumentOptions
            {
                CommentHandling = JsonCommentHandling.Skip
            });
            using var defaultsDocument = JsonDocument.Parse(defaults, new JsonDocumentOptions
            {
                CommentHandling = JsonCommentHandling.Skip
            });

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

            if (writer.BytesCommitted == 0)
            {
                File.Delete(filePath);
            }
        }
    }
}
