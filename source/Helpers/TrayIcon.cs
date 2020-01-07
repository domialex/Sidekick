using System.Reflection;
using Sidekick.Windows.ApplicationLogs;
using System.Windows.Forms;
using System.Windows.Media.Imaging;
using System;
using System.Drawing;
using System.Runtime.Versioning;

namespace Sidekick.Helpers
{
    public static class TrayIcon
    {
        static private NotifyIcon _notifyIcon;

        public static void Initialize()
        {
            _notifyIcon = new NotifyIcon();
            var icon = Resources.ExaltedOrb;
            _notifyIcon.Icon = icon;
            _notifyIcon.Visible = true;
            _notifyIcon.Text = "Sidekick";

            var contextMenu = new ContextMenuStrip();
            contextMenu.Items.Add("Show logs", null, (s, e) => ApplicationLogsController.Show());
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
