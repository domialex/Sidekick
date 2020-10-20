using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Sidekick.Core.Initialization.Notifications;

namespace Sidekick.Core.Natives
{
    public class InitializeKeybindsHandler : INotificationHandler<InitializeKeybindsNotification>
    {
        private readonly INativeProcess nativeProcess;

        public InitializeKeybindsHandler(INativeProcess nativeProcess)
        {
            this.nativeProcess = nativeProcess;
        }

        public Task Handle(InitializeKeybindsNotification notification, CancellationToken cancellationToken)
        {
            notification.OnStart("Sidekick.Core.Natives");
            Task.Run(nativeProcess.CheckPermission);
            notification.OnEnd("Sidekick.Core.Natives");
            return Task.CompletedTask;
        }
    }
}
