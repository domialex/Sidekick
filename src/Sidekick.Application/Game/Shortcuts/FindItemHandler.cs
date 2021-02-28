using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Sidekick.Domain.Game.Items.Commands;
using Sidekick.Domain.Game.Shortcuts.Commands;
using Sidekick.Domain.Platforms;

namespace Sidekick.Application.Game.Shortcuts
{
    public class FindItemHandler : ICommandHandler<FindItemCommand, bool>
    {
        private readonly IKeyboardProvider keyboard;
        private readonly IClipboardProvider clipboardProvider;
        private readonly IMediator mediator;

        public FindItemHandler(
            IKeyboardProvider keyboard,
            IClipboardProvider clipboardProvider,
            IMediator mediator)
        {
            this.keyboard = keyboard;
            this.clipboardProvider = clipboardProvider;
            this.mediator = mediator;
        }

        public async Task<bool> Handle(FindItemCommand request, CancellationToken cancellationToken)
        {
            var text = await clipboardProvider.Copy();
            var item = await mediator.Send(new ParseItemCommand(text));

            if (item != null)
            {
                var clipboardContents = await clipboardProvider.GetText();

                await clipboardProvider.SetText(item.Original.Name);

                keyboard.PressKey("Ctrl+F", "Ctrl+A", "Paste", "Enter");

                await Task.Delay(100);
                await clipboardProvider.SetText(clipboardContents);

                return true;
            }

            return false;
        }
    }
}
