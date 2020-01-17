using System;

namespace Sidekick.Business.Notifications
{
    public class NotificationEvent : EventArgs
    {
        public Notification Notification { get; set; }
    }
}
