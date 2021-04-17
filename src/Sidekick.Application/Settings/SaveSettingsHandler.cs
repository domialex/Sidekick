using System;
using System.IO;
using System.Reflection;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Sidekick.Domain.Cache.Commands;
using Sidekick.Domain.Game.Languages.Commands;
using Sidekick.Domain.Initialization.Commands;
using Sidekick.Domain.Localization;
using Sidekick.Domain.Platforms;
using Sidekick.Domain.Settings.Commands;
using Sidekick.Extensions;

namespace Sidekick.Application.Settings
{
    public class SaveSettingsHandler : ICommandHandler<SaveSettingsCommand>
    {
        public const string FileName = "Sidekick_settings.json";
        private readonly IMediator mediator;
        private readonly SidekickSettings settings;
        private readonly IKeybindProvider keybindProvider;

        public SaveSettingsHandler(
            IMediator mediator,
            SidekickSettings settings,
            IKeybindProvider keybindProvider)
        {
            this.mediator = mediator;
            this.settings = settings;
            this.keybindProvider = keybindProvider;
        }

        public async Task<Unit> Handle(SaveSettingsCommand request, CancellationToken cancellationToken)
        {
            var leagueHasChanged = request.Settings.LeagueId != settings.LeagueId;
            var languageHasChanged = request.Settings.Language_Parser != settings.Language_Parser;

            if (settings.Language_UI != request.Settings.Language_UI)
            {
                await mediator.Send(new SetUiLanguageCommand(request.Settings.Language_UI));
            }
            if (settings.Language_Parser != request.Settings.Language_Parser)
            {
                await mediator.Send(new SetGameLanguageCommand(request.Settings.Language_Parser));
            }

            request.Settings.CopyValuesTo(settings);

            var json = JsonSerializer.Serialize(settings);
            var defaults = JsonSerializer.Serialize(new SidekickSettings());
            var sidekickPath = Environment.ExpandEnvironmentVariables("%AppData%\\sidekick");
            var filePath = Path.Combine(sidekickPath, FileName);

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

            if (!request.SkipInitialize && (languageHasChanged || leagueHasChanged))
            {
                await mediator.Send(new ClearCacheCommand());
                await mediator.Send(new InitializeCommand(false));
            }

            return Unit.Value;
        }
    }
}
