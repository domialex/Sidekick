using System;

namespace Sidekick.Business.Notifications
{
    public class NotificationService : INotificationService
    {
        public event EventHandler TrayNotified;

        public void NotifyTray(Notification notification)
        {
            TrayNotified?.Invoke(null, new NotificationEvent()
            {
                Notification = notification
            });
        }
    }
}
