using System;

namespace Sidekick.Business.Notifications
{
    public interface INotificationService
    {
        event EventHandler TrayNotified;

        void NotifyTray(Notification notification);
    }
}
