using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Sidekick.Domain.Initialization.Notifications;
using Sidekick.Domain.Keybinds;
using Sidekick.Domain.Platforms;

namespace Sidekick.Platform
{
    public class PlatformInitializationStartedHandler : INotificationHandler<KeybindsInitializationStarted>
    {
        private readonly IProcessProvider processProvider;
        private readonly IKeybindsProvider keybindsProvider;
        private readonly IKeybindsExecutor keybindsExecutor;
        private readonly IScrollProvider scrollProvider;

        public PlatformInitializationStartedHandler(
            IProcessProvider processProvider,
            IKeybindsProvider keybindsProvider,
            IKeybindsExecutor keybindsExecutor,
            IScrollProvider scrollProvider)
        {
            this.processProvider = processProvider;
            this.keybindsProvider = keybindsProvider;
            this.keybindsExecutor = keybindsExecutor;
            this.scrollProvider = scrollProvider;
        }

        public Task Handle(KeybindsInitializationStarted notification, CancellationToken cancellationToken)
        {
            Task.Run(processProvider.CheckPermission, cancellationToken);
            keybindsProvider.Initialize();
            scrollProvider.Initialize();
            keybindsExecutor.Initialize();

            return Unit.Task;
        }
    }
}
