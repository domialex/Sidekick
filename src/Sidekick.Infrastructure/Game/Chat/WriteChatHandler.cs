using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Sidekick.Domain.Clipboard;
using Sidekick.Domain.Game.Chat.Commands;
using Sidekick.Domain.Keybinds;
using Sidekick.Domain.Settings;

namespace Sidekick.Infrastructure.Game.Chat.Commands
{
    public class WriteChatHandler : ICommandHandler<WriteChatCommand>
    {
        private readonly ISidekickSettings settings;
        private readonly IClipboardProvider clipboard;
        private readonly IKeybindsProvider keybindsProvider;

        public WriteChatHandler(
            ISidekickSettings settings,
            IClipboardProvider clipboard,
            IKeybindsProvider keybindsProvider)
        {
            this.settings = settings;
            this.clipboard = clipboard;
            this.keybindsProvider = keybindsProvider;
        }

        public async Task<Unit> Handle(WriteChatCommand request, CancellationToken cancellationToken)
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
            keybindsProvider.PressKey("Enter");
            keybindsProvider.PressKey("Enter");
            keybindsProvider.PressKey("Up");
            keybindsProvider.PressKey("Up");
            keybindsProvider.PressKey("Esc");

            if (settings.RetainClipboard)
            {
                await Task.Delay(100);
                await clipboard.SetText(clipboardValue);
            }

            return Unit.Value;
        }
    }
}
