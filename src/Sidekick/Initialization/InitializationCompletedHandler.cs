using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Sidekick.Core.Natives;
using Sidekick.Core.Settings;
using Sidekick.Domain.Initialization.Notifications;
using Sidekick.Localization.Tray;

namespace Sidekick.Initialization
{
    public class InitializationCompletedHandler : INotificationHandler<InitializationCompleted>
    {
        private readonly SidekickSettings settings;
        private readonly InitializationViewModel viewModel;
        private readonly INativeNotifications nativeNotifications;

        public InitializationCompletedHandler(
            SidekickSettings settings,
            InitializationViewModel viewModel,
            INativeNotifications nativeNotifications)
        {
            this.settings = settings;
            this.viewModel = viewModel;
            this.nativeNotifications = nativeNotifications;
        }

        public async Task Handle(InitializationCompleted notification, CancellationToken cancellationToken)
        {
            await Task.Delay(400);
            viewModel.Complete();

            nativeNotifications.ShowSystemNotification(
                TrayResources.Notification_Title,
                string.Format(TrayResources.Notification_Message, settings.Key_CheckPrices.ToKeybindString(), settings.Key_CloseWindow.ToKeybindString())
            );
        }
    }
}
