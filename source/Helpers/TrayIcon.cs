using Sidekick.Helpers.POETradeAPI;
using Sidekick.Helpers.POETradeAPI.Models.TradeData;
using Sidekick.Windows.ApplicationLogs;
using Sidekick.Windows.Settings;
using System.Collections.Generic;
using System.Windows.Forms;


namespace Sidekick.Helpers
{
    public static class TrayIcon
    {
        private static NotifyIcon _notifyIcon;
        private static ToolStripMenuItem _leagueSelectMenu;

        public static void Initialize()
        {
            _notifyIcon = new NotifyIcon();
            var icon = Resources.ExaltedOrb;
            _notifyIcon.Icon = icon;
            _notifyIcon.Visible = true;
            _notifyIcon.Text = "Sidekick";

            var contextMenu = new ContextMenuStrip();
            _leagueSelectMenu = new ToolStripMenuItem("League");
            contextMenu.Items.Add(_leagueSelectMenu);
            contextMenu.Items.Add(new ToolStripSeparator());
            contextMenu.Items.Add("Settings", null, (s, e) => SettingsController.Show());
            contextMenu.Items.Add("Show logs", null, (s, e) => ApplicationLogsController.Show());
            contextMenu.Items.Add("Exit", null, (s, e) => Application.Exit());
            _notifyIcon.ContextMenuStrip = contextMenu;
        }

        public static void PopulateLeagueSelectMenu(List<League> leagues)
        {
            foreach (League l in leagues)
            {
                var menuItem = new ToolStripMenuItem(l.Id);
                menuItem.Click += (s, e) => { foreach (ToolStripMenuItem t in _leagueSelectMenu.DropDownItems) { t.Checked = false; } };
                menuItem.Click += (s, e) => { menuItem.Checked = true; };
                menuItem.Click += (s, e) => { TradeClient.SelectedLeague = l; };
                _leagueSelectMenu.DropDownItems.Add(menuItem);
            }
            //select the first league as the default
            _leagueSelectMenu.DropDownItems[0].PerformClick();
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
