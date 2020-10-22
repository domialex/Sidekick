using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Sidekick.Core.Natives;
using Sidekick.Core.Settings;
using Sidekick.Domain.Initialization.Notifications;
using Sidekick.Localization.Tray;
using Sidekick.Views;

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
            await Task.Delay(500);
            viewLocator.Close<InitializationView>();

            nativeNotifications.ShowSystemNotification(
                TrayResources.Notification_Title,
                string.Format(TrayResources.Notification_Message, settings.Key_CheckPrices.ToKeybindString(), settings.Key_CloseWindow.ToKeybindString())
            );
        }
    }
}
