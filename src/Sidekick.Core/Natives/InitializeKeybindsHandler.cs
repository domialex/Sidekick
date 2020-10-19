using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Sidekick.Core.Initialization;

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
            Task.Run(nativeProcess.CheckPermission);
            return Task.CompletedTask;
        }
    }
}
