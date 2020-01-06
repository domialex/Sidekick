using Sidekick.Helpers.NativeMethods;
using Sidekick.Windows.Logs;
using System.Windows.Forms;

namespace Sidekick.Helpers
{
    public static class TrayIcon
    {
        static private NotifyIcon _notifyIcon;

        public static void Initialize()
        {
            _notifyIcon = new NotifyIcon();
            _notifyIcon.Icon = new System.Drawing.Icon("Resources/ExaltedOrb.ico");
            _notifyIcon.Visible = true;
            _notifyIcon.Text = "Sidekick";

            var contextMenu = new ContextMenuStrip();
            contextMenu.Items.Add("Show logs", null, (s, e) => LogsController.Show());
            contextMenu.Items.Add("Exit", null, (s, e) => Application.Exit());
            _notifyIcon.ContextMenuStrip = contextMenu;
        }

        public static void SendNotification(string text, string title = null)
        {
            _notifyIcon.BalloonTipTitle = title;
            _notifyIcon.BalloonTipText = text;
            _notifyIcon.ShowBalloonTip(2000);
        }

        public static void Dispose()
        {
            _notifyIcon?.Dispose();
        }
    }
}
