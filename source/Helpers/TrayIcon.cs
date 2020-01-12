using Sidekick.Helpers.POETradeAPI;
using Sidekick.Helpers.POETradeAPI.Models.TradeData;
using Sidekick.Windows.ApplicationLogs;
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
            contextMenu.Items.Add("Show logs", null, (s, e) => ApplicationLogsController.Show());
            contextMenu.Items.Add("Exit", null, (s, e) => Application.Exit());
            _notifyIcon.ContextMenuStrip = contextMenu;
        }

        public static void PopulateLeagueSelectMenu(List<League> leagues)
        {
            if(_leagueSelectMenu.DropDownItems.Count > 0)
            {
                // TODO: Fix Cross-thread operation not valid after changing language.
                return;
            }

            foreach (var league in leagues)
            {
                var menuItem = new ToolStripMenuItem(league.Id);
                menuItem.Click += (s, e) => { foreach (ToolStripMenuItem x in _leagueSelectMenu.DropDownItems) { x.Checked = false; } };
                menuItem.Click += (s, e) => { menuItem.Checked = true; };
                menuItem.Click += (s, e) => { TradeClient.SelectedLeague = league; };
                _leagueSelectMenu.DropDownItems.Add(menuItem);
            }

            // Select the first league as the default.
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
