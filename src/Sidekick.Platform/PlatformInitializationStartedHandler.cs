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
        private readonly IKeyboardProvider keybindsProvider;
        private readonly IKeybindsExecutor keybindsExecutor;
        private readonly IScrollProvider scrollProvider;
        private readonly IMouseProvider mouseProvider;
        private readonly IScreenProvider screenProvider;

        public PlatformInitializationStartedHandler(
            IProcessProvider processProvider,
            IKeyboardProvider keybindsProvider,
            IKeybindsExecutor keybindsExecutor,
            IScrollProvider scrollProvider,
            IMouseProvider mouseProvider,
            IScreenProvider screenProvider)
        {
            this.processProvider = processProvider;
            this.keybindsProvider = keybindsProvider;
            this.keybindsExecutor = keybindsExecutor;
            this.scrollProvider = scrollProvider;
            this.mouseProvider = mouseProvider;
            this.screenProvider = screenProvider;
        }

        public Task Handle(KeybindsInitializationStarted notification, CancellationToken cancellationToken)
        {
            // Task.Run(processProvider.CheckPermission, cancellationToken);
            keybindsProvider.Initialize();
            scrollProvider.Initialize();
            keybindsExecutor.Initialize();
            mouseProvider.Initialize();
            screenProvider.Initialize();

            return Unit.Task;
        }
    }
}
