using System;
using Sidekick.Core.Natives;

namespace Sidekick.Natives
{
    public class NativeNotifications : INativeNotifications
    {
        private readonly App app;

        public NativeNotifications(App app)
        {
            this.app = app;
        }

        public void ShowMessage(string message, string title = null)
        {
            app.Dispatcher.Invoke(() =>
            {
                AdonisUI.Controls.MessageBox.Show(message, title, buttons: AdonisUI.Controls.MessageBoxButton.OK);
            });
        }

        public void ShowYesNo(string message, string title = null, Action onYes = null, Action onNo = null)
        {
            app.Dispatcher.Invoke(() =>
            {
                var result = AdonisUI.Controls.MessageBox.Show(message, title, AdonisUI.Controls.MessageBoxButton.YesNo);
                if (onYes != null && result == AdonisUI.Controls.MessageBoxResult.Yes)
                {
                    onYes.Invoke();
                }
                if (onNo != null && result == AdonisUI.Controls.MessageBoxResult.No)
                {
                    onNo.Invoke();
                }
            });
        }

        public void ShowSystemNotification(string title, string message)
        {
            app.Dispatcher.Invoke(() =>
            {
                app.TrayIcon.ShowBalloonTip(
                    title,
                    message,
                    app.TrayIcon.Icon,
                    largeIcon: true);
            });
        }
    }
}
