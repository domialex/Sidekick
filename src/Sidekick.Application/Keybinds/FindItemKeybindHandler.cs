using System.Threading.Tasks;
using MediatR;
using Sidekick.Common.Platform;
using Sidekick.Domain.Game.Items.Commands;
using Sidekick.Domain.Keybinds;

namespace Sidekick.Application.Keybinds
{
    public class FindItemKeybindHandler : IKeybindHandler
    {
        private readonly IKeyboardProvider keyboard;
        private readonly IClipboardProvider clipboardProvider;
        private readonly IMediator mediator;
        private readonly IProcessProvider processProvider;

        public FindItemKeybindHandler(
            IKeyboardProvider keyboard,
            IClipboardProvider clipboardProvider,
            IMediator mediator,
            IProcessProvider processProvider)
        {
            this.keyboard = keyboard;
            this.clipboardProvider = clipboardProvider;
            this.mediator = mediator;
            this.processProvider = processProvider;
        }

        public bool IsValid() => processProvider.IsPathOfExileInFocus;

        public async Task Execute()
        {
            var text = await clipboardProvider.Copy();
            var item = await mediator.Send(new ParseItemCommand(text));

            if (item != null)
            {
                await clipboardProvider.SetText(item.Original.Name);
                keyboard.PressKey("Ctrl+F", "Ctrl+A", "Paste", "Enter");
            }
            else
            {
                // If we are not hovering over an item, we still want to put the focus in the search bar inside the game.
                keyboard.PressKey("Ctrl+F");
            }
        }
    }
}
