using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Sidekick.Domain.Game.Chat.Commands;
using Sidekick.Domain.Platforms;
using Sidekick.Domain.Settings;

namespace Sidekick.Application.Game.Chat.Commands
{
    public class StartWritingChatHandler : ICommandHandler<StartWritingChatCommand>
    {
        private readonly ISidekickSettings settings;
        private readonly IClipboardProvider clipboard;
        private readonly IKeyboardProvider keyboard;

        public StartWritingChatHandler(
            ISidekickSettings settings,
            IClipboardProvider clipboard,
            IKeyboardProvider keyboard)
        {
            this.settings = settings;
            this.clipboard = clipboard;
            this.keyboard = keyboard;
        }

        public async Task<Unit> Handle(StartWritingChatCommand request, CancellationToken cancellationToken)
        {
            string clipboardValue = null;
            if (settings.RetainClipboard)
            {
                clipboardValue = await clipboard.GetText();
            }

            await clipboard.SetText(request.Message);

            await keyboard.PressKey("Enter", "Ctrl+A", "Paste");

            if (settings.RetainClipboard)
            {
                await Task.Delay(100);
                await clipboard.SetText(clipboardValue);
            }

            return Unit.Value;
        }
    }
}
