using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Logging;
using Sidekick.Domain.Game.Chat.Commands;
using Sidekick.Domain.Game.GameLogs.Queries;
using Sidekick.Domain.Platforms;
using Sidekick.Domain.Settings;

namespace Sidekick.Application.Game.Chat.Commands
{
    public class ChatHandler : ICommandHandler<ChatCommand, bool>
    {
        private const string Token_Me_CharacterName = "{Me.CharacterName}";
        private const string Token_LastWhisper_CharacterName = "{LastWhisper.CharacterName}";

        private readonly ISidekickSettings settings;
        private readonly IClipboardProvider clipboard;
        private readonly IKeyboardProvider keyboard;
        private readonly ILogger<ChatHandler> logger;
        private readonly IMediator mediator;

        public ChatHandler(
            ISidekickSettings settings,
            IClipboardProvider clipboard,
            IKeyboardProvider keyboard,
            ILogger<ChatHandler> logger,
            IMediator mediator)
        {
            this.settings = settings;
            this.clipboard = clipboard;
            this.keyboard = keyboard;
            this.logger = logger;
            this.mediator = mediator;
        }

        public async Task<bool> Handle(ChatCommand request, CancellationToken cancellationToken)
        {
            if (string.IsNullOrEmpty(request.Command))
            {
                return false;
            }

            string clipboardValue = null;
            if (settings.RetainClipboard)
            {
                clipboardValue = await clipboard.GetText();
            }

            var command = request.Command;

            if (request.Command.Contains(Token_Me_CharacterName))
            {
                // This operation is only valid if the user has added their character name to the settings file.
                if (string.IsNullOrEmpty(settings.Character_Name))
                {
                    logger.LogWarning(@"This command requires a ""CharacterName"" to be specified in the settings menu.");
                    return false;
                }

                command = command.Replace(Token_Me_CharacterName, settings.Character_Name);
            }

            if (request.Command.Contains(Token_LastWhisper_CharacterName))
            {
                var characterName = await mediator.Send(new GetLatestWhisperCharacterNameQuery());
                if (string.IsNullOrEmpty(characterName))
                {
                    logger.LogWarning(@"No last whisper was found in the log file.");
                    return false;
                }

                command = command.Replace(Token_LastWhisper_CharacterName, characterName);
            }

            await clipboard.SetText(command);

            if (request.Submit)
            {
                keyboard.PressKey("Enter", "Ctrl+A", "Paste", "Enter", "Enter", "Up", "Up", "Esc");
            }
            else
            {
                keyboard.PressKey("Enter", "Ctrl+A", "Paste");
            }

            if (settings.RetainClipboard)
            {
                await Task.Delay(100);
                await clipboard.SetText(clipboardValue);
            }

            return true;
        }
    }
}
