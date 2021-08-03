using System.IO;
using System.Text.Json;
using System.Threading.Tasks;
using MediatR;
using Sidekick.Common.Settings;
using Sidekick.Domain.Cache.Commands;
using Sidekick.Domain.Game.Languages.Commands;
using Sidekick.Domain.Initialization.Commands;
using Sidekick.Domain.Localization;
using Sidekick.Extensions;

namespace Sidekick.Modules.Settings
{
    public class SettingsService : ISettingsService
    {
        public const string FileName = "Sidekick_settings.json";
        private readonly ISettings settings;
        private readonly IMediator mediator;

        public SettingsService(
            ISettings settings,
            IMediator mediator)
        {
            this.settings = settings;
            this.mediator = mediator;
        }

        public async Task Save(string property, object value)
        {
            var propertyType = settings.GetType().GetProperty(property);
            var propertyValue = propertyType.GetValue(settings);

            if (propertyValue != null && value != null && value.Equals(propertyValue))
            {
                return;
            }

            var newSettings = new Settings();
            settings.CopyValuesTo(newSettings);
            propertyType.SetValue(newSettings, value);
            await Save(newSettings, true);
        }

        public async Task Save(ISettings newSettings, bool skipInitialize)
        {
            var leagueHasChanged = settings.LeagueId != newSettings.LeagueId;
            var languageHasChanged = settings.Language_Parser != newSettings.Language_Parser;

            if (settings.Language_UI != newSettings.Language_UI)
            {
                await mediator.Send(new SetUiLanguageCommand(newSettings.Language_UI));
            }
            if (languageHasChanged)
            {
                await mediator.Send(new SetGameLanguageCommand(newSettings.Language_Parser));
            }

            newSettings.CopyValuesTo(settings);

            var json = JsonSerializer.Serialize(settings);
            var defaults = JsonSerializer.Serialize(new Settings());
            var filePath = SidekickPaths.GetDataFilePath(FileName);

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

            if (!skipInitialize && (languageHasChanged || leagueHasChanged))
            {
                await mediator.Send(new ClearCacheCommand());
                await mediator.Send(new InitializeCommand(false, false));
            }
        }
    }
}
