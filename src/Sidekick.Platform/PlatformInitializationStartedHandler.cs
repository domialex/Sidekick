using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Sidekick.Domain.Initialization.Notifications;
using Sidekick.Domain.Keybinds;
using Sidekick.Domain.Platforms;

namespace Sidekick.Platform
{
    public class PlatformInitializationStartedHandler : INotificationHandler<PlatformInitializationStarted>
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

        public async Task Handle(PlatformInitializationStarted notification, CancellationToken cancellationToken)
        {
            await processProvider.Initialize(cancellationToken);
            keybindsProvider.Initialize();
            scrollProvider.Initialize();
            mouseProvider.Initialize();
            screenProvider.Initialize();
            keybindsExecutor.Initialize();
        }
    }
}
