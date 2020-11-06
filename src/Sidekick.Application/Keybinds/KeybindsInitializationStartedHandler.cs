using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Sidekick.Domain.Initialization.Notifications;
using Sidekick.Domain.Keybinds;
using Sidekick.Domain.Process;

namespace Sidekick.Application.Keybinds
{
    public class KeybindsInitializationStartedHandler : INotificationHandler<KeybindsInitializationStarted>
    {
        private readonly INativeProcess nativeProcess;
        private readonly IKeybindsProvider keybindsProvider;
        private readonly IKeybindsExecutor keybindsExecutor;

        public KeybindsInitializationStartedHandler(
            INativeProcess nativeProcess,
            IKeybindsProvider keybindsProvider,
            IKeybindsExecutor keybindsExecutor)
        {
            this.nativeProcess = nativeProcess;
            this.keybindsProvider = keybindsProvider;
            this.keybindsExecutor = keybindsExecutor;
        }

        public Task Handle(KeybindsInitializationStarted notification, CancellationToken cancellationToken)
        {
            Task.Run(nativeProcess.CheckPermission);
            keybindsProvider.Initialize();
            keybindsExecutor.Initialize();

            return Unit.Task;
        }
    }
}
