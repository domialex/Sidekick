using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Sidekick.Domain.Game.Stashes.Commands;
using Sidekick.Domain.Platforms;

namespace Sidekick.Application.Game.Stashes
{
    public class ScrollStashDownHandler : ICommandHandler<ScrollStashDownCommand, bool>
    {
        private readonly IKeybindsProvider keybindsProvider;

        public ScrollStashDownHandler(
            IKeybindsProvider keybindsProvider)
        {
            this.keybindsProvider = keybindsProvider;
        }

        public async Task<bool> Handle(ScrollStashDownCommand request, CancellationToken cancellationToken)
        {
            await keybindsProvider.PressKey("Right");
            return true;
        }
    }
}
