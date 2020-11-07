using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Sidekick.Domain.Clipboard;
using Sidekick.Domain.Game.Chat.Commands;
using Sidekick.Domain.Keybinds;
using Sidekick.Domain.Settings;

namespace Sidekick.Infrastructure.Game.Chat.Commands
{
    public class StartWritingChatHandler : ICommandHandler<StartWritingChatCommand>
    {
        private readonly ISidekickSettings settings;
        private readonly INativeClipboard clipboard;
        private readonly IKeybindsProvider keybindsProvider;

        public StartWritingChatHandler(
            ISidekickSettings settings,
            INativeClipboard clipboard,
            IKeybindsProvider keybindsProvider)
        {
            this.settings = settings;
            this.clipboard = clipboard;
            this.keybindsProvider = keybindsProvider;
        }

        public async Task<Unit> Handle(StartWritingChatCommand request, CancellationToken cancellationToken)
        {
            string clipboardValue = null;
            if (settings.RetainClipboard)
            {
                clipboardValue = await clipboard.GetText();
            }

            await clipboard.SetText(request.Message);

            keybindsProvider.PressKey("Enter");
            keybindsProvider.PressKey("Ctrl+A");
            keybindsProvider.PressKey("Paste");

            if (settings.RetainClipboard)
            {
                await Task.Delay(100);
                await clipboard.SetText(clipboardValue);
            }

            return Unit.Value;
        }
    }
}
