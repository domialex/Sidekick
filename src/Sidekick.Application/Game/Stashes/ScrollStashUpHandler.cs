using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Sidekick.Domain.Game.Stashes.Commands;
using Sidekick.Domain.Platforms;

namespace Sidekick.Application.Game.Stashes
{
    public class ScrollStashUpHandler : ICommandHandler<ScrollStashUpCommand, bool>
    {
        private readonly IKeybindsProvider keybindsProvider;

        public ScrollStashUpHandler(
            IKeybindsProvider keybindsProvider)
        {
            this.keybindsProvider = keybindsProvider;
        }

        public Task<bool> Handle(ScrollStashUpCommand request, CancellationToken cancellationToken)
        {
            keybindsProvider.PressKey("Left");
            return Task.FromResult(true);
        }
    }
}
