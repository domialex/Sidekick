using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Sidekick.Domain.Game.Stashes.Commands;
using Sidekick.Domain.Keybinds;
using Sidekick.Domain.Process;

namespace Sidekick.Infrastructure.Game.Stashes
{
    public class ScrollStashUpHandler : ICommandHandler<ScrollStashUpCommand, bool>
    {
        private readonly IKeybindsProvider keybindsProvider;
        private readonly INativeProcess nativeProcess;

        public ScrollStashUpHandler(
            IKeybindsProvider keybindsProvider,
            INativeProcess nativeProcess)
        {
            this.keybindsProvider = keybindsProvider;
            this.nativeProcess = nativeProcess;
        }

        public Task<bool> Handle(ScrollStashUpCommand request, CancellationToken cancellationToken)
        {
            if (!nativeProcess.IsPathOfExileInFocus)
            {
                return Task.FromResult(false);
            }

            keybindsProvider.PressKey("Left");
            return Task.FromResult(true);
        }
    }
}
