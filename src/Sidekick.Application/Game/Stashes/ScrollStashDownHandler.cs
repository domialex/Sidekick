using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Sidekick.Domain.Game.Stashes.Commands;
using Sidekick.Domain.Keybinds;
using Sidekick.Domain.Process;

namespace Sidekick.Application.Game.Stashes
{
    public class ScrollStashDownHandler : ICommandHandler<ScrollStashDownCommand, bool>
    {
        private readonly IKeybindsProvider keybindsProvider;
        private readonly INativeProcess nativeProcess;

        public ScrollStashDownHandler(
            IKeybindsProvider keybindsProvider,
            INativeProcess nativeProcess)
        {
            this.keybindsProvider = keybindsProvider;
            this.nativeProcess = nativeProcess;
        }

        public Task<bool> Handle(ScrollStashDownCommand request, CancellationToken cancellationToken)
        {
            if (!nativeProcess.IsPathOfExileInFocus)
            {
                return Task.FromResult(false);
            }

            keybindsProvider.PressKey("Right");
            return Task.FromResult(true);
        }
    }
}
