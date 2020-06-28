using System;

namespace Sidekick.Notifications
{
    public interface INotificationManager
    {
        void ShowMessage(string message, string title = null);
        void ShowYesNo(string message, string title = null, Action onYes = null, Action onNo = null);
        void ShowSystemNotification(string title, string message);
    }
}
