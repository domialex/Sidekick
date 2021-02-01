using System.IO;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Sidekick.Domain.Cache.Commands;
using Sidekick.Domain.Game.Languages;
using Sidekick.Domain.Game.Languages.Commands;
using Sidekick.Domain.Initialization.Commands;
using Sidekick.Domain.Localization;
using Sidekick.Domain.Settings.Commands;
using Sidekick.Extensions;

namespace Sidekick.Application.Settings
{
    public class SaveSettingsHandler : ICommandHandler<SaveSettingsCommand>
    {
        internal const string FileName = "Sidekick_settings.json";
        private readonly IMediator mediator;
        private readonly SidekickSettings settings;
        private readonly IGameLanguageProvider gameLanguageProvider;
        private readonly IUILanguageProvider uiLanguageProvider;

        public SaveSettingsHandler(
            IMediator mediator,
            SidekickSettings settings,
            IGameLanguageProvider gameLanguageProvider,
            IUILanguageProvider uiLanguageProvider)
        {
            this.mediator = mediator;
            this.settings = settings;
            this.gameLanguageProvider = gameLanguageProvider;
            this.uiLanguageProvider = uiLanguageProvider;
        }

        public async Task<Unit> Handle(SaveSettingsCommand request, CancellationToken cancellationToken)
        {
            var leagueHasChanged = request.Settings.LeagueId != settings.LeagueId;
            var languageHasChanged = gameLanguageProvider.Current.LanguageCode != request.Settings.Language_Parser;

            uiLanguageProvider.SetLanguage(request.Settings.Language_UI);
            await mediator.Send(new SetGameLanguageCommand(request.Settings.Language_Parser));

            request.Settings.CopyValuesTo(settings);

            var json = JsonSerializer.Serialize(settings);
            var defaults = JsonSerializer.Serialize(new SidekickSettings());
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
                return Unit.Value;
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

            if (languageHasChanged || leagueHasChanged)
            {
                await mediator.Send(new ClearCacheCommand());
            }

            return Unit.Value;
        }
    }
}
