using System;

namespace Sidekick.Core.Natives
{
    public interface INativeNotifications
    {
        void ShowMessage(string message, string title = null);
        void ShowYesNo(string message, string title = null, Action onYes = null, Action onNo = null);
        void ShowSystemNotification(string title, string message);
    }
}
