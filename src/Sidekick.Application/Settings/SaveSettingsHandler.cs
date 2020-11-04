using System.IO;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Sidekick.Domain.Settings.Commands;
using Sidekick.Extensions;

namespace Sidekick.Application.Settings
{
    public class SaveSettingsHandler : ICommandHandler<SaveSettingsCommand>
    {
        internal const string FileName = "Sidekick_settings.json";
        private readonly SidekickSettings settings;

        public SaveSettingsHandler(SidekickSettings settings)
        {
            this.settings = settings;
        }

        public Task<Unit> Handle(SaveSettingsCommand request, CancellationToken cancellationToken)
        {
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
                return Unit.Task;
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

            return Unit.Task;
        }
    }
}
