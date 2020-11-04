using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Sidekick.Domain.Initialization.Notifications;

namespace Sidekick.Core.Natives
{
    public class KeybindsInitializationStartedHandler : INotificationHandler<KeybindsInitializationStarted>
    {
        private readonly INativeProcess nativeProcess;

        public KeybindsInitializationStartedHandler(INativeProcess nativeProcess)
        {
            this.nativeProcess = nativeProcess;
        }

        public Task Handle(KeybindsInitializationStarted notification, CancellationToken cancellationToken)
        {
            Task.Run(nativeProcess.CheckPermission);
            return Task.CompletedTask;
        }
    }
}
