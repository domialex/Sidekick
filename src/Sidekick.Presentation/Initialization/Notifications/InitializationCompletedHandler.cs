using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Sidekick.Core.Natives;
using Sidekick.Core.Settings;
using Sidekick.Domain.Initialization.Notifications;
using Sidekick.Localization.Tray;
using Sidekick.Presentation.Views;

namespace Sidekick.Initialization
{
    public class InitializationCompletedHandler : INotificationHandler<InitializationCompleted>
    {
        private readonly SidekickSettings settings;
        private readonly INativeNotifications nativeNotifications;
        private readonly IViewLocator viewLocator;

        public InitializationCompletedHandler(
            SidekickSettings settings,
            INativeNotifications nativeNotifications,
            IViewLocator viewLocator)
        {
            this.settings = settings;
            this.nativeNotifications = nativeNotifications;
            this.viewLocator = viewLocator;
        }

        public async Task Handle(InitializationCompleted notification, CancellationToken cancellationToken)
        {
            // If we have a successful initialization, we delay for half a second to show the "Ready" label on the UI before closing the view
            await Task.Delay(500);

            // Show a system notification
            nativeNotifications.ShowSystemNotification(
                TrayResources.Notification_Title,
                string.Format(TrayResources.Notification_Message, settings.Key_CheckPrices.ToKeybindString(), settings.Key_CloseWindow.ToKeybindString())
            );

            viewLocator.Close(View.Initialization);
        }
    }
}
