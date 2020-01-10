using Sidekick.Helpers.POETradeAPI.Models.TradeData;
using Sidekick.Helpers.POETradeAPI;
using Sidekick.Windows.ApplicationLogs;
using System.Collections.Generic;
using System.Windows.Forms;
using System;


namespace Sidekick.Helpers
{
    public interface ITrayIcon : IDisposable
    {
        void Initialize();
        void PopulateLeagueSelectMenu(List<League> leagues);
        void SendNotification(string text, string title = null);
    }

    public class DefaultTrayIcon : ITrayIcon
    {
        private NotifyIcon _notifyIcon;
        private ToolStripMenuItem _leagueSelectMenu;

        public void Initialize()
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

        public void PopulateLeagueSelectMenu(List<League> leagues)
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

        public void SendNotification(string text, string title = null)
        {
            _notifyIcon.BalloonTipTitle = title;
            _notifyIcon.BalloonTipText = text;
            _notifyIcon.ShowBalloonTip(2000);
        }

        public void Dispose()
        {
            _notifyIcon?.Dispose();
        }
    }

    public static class TrayIcon
    {
        static private NotifyIcon _notifyIcon;
        static private ToolStripMenuItem _leagueSelectMenu;

        public static ITrayIcon UsedTrayIcon = new DefaultTrayIcon();

        public static void Initialize()
        {
            UsedTrayIcon.Initialize();
        }

        public static void PopulateLeagueSelectMenu(List<League> leagues) 
        {
            UsedTrayIcon.PopulateLeagueSelectMenu(leagues);
        }

        public static void SendNotification(string text, string title = null)
        {
            UsedTrayIcon.SendNotification(text, title);
        }

        public static void Dispose()
        {
            UsedTrayIcon.Dispose();
        }
    }
}
