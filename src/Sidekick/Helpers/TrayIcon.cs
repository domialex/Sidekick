using Sidekick.Business.Leagues.Models;
using Sidekick.Core.Settings;
using Sidekick.Windows.ApplicationLogs;
using Sidekick.Windows.Settings;
using System;
using System.Collections.Generic;
using System.Linq;
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

            ReloadUI();

            Legacy.UILanguageProvider.UILanguageChanged.Add(ReloadUI);
        }

        public static void ReloadUI()
        {
            var settings = SettingsController.GetSettingsInstance();
            var contextMenu = new ContextMenuStrip();
            _leagueSelectMenu = new ToolStripMenuItem("League");
            contextMenu.Items.Add(_leagueSelectMenu);
            contextMenu.Items.Add(new ToolStripSeparator());
            contextMenu.Items.Add(settings.CurrentUILanguageProvider.UILanguage.TrayIconSettings, null, (s, e) => SettingsController.Show());
            contextMenu.Items.Add(settings.CurrentUILanguageProvider.UILanguage.TrayIconShowLogs, null, (s, e) => ApplicationLogsController.Show());
            contextMenu.Items.Add(settings.CurrentUILanguageProvider.UILanguage.TrayIconExit, null, (s, e) => Application.Exit());
            _notifyIcon.ContextMenuStrip = contextMenu;
        }

        public static void PopulateLeagueSelectMenu(List<League> leagues)
        {
            if (leagues == null)
            {
                return;
            }

            foreach (var league in leagues)
            {
                var menuItem = new ToolStripMenuItem(league.Id);
                menuItem.Click += (s, e) => { foreach (ToolStripMenuItem x in _leagueSelectMenu.DropDownItems) { x.Checked = false; } };
                menuItem.Click += (s, e) => { menuItem.Checked = true; };
                menuItem.Click += (s, e) => { Legacy.LeagueService.SelectedLeague = league; };
                _leagueSelectMenu.DropDownItems.Add(menuItem);
            }

            // Select the first league as the default.
            _leagueSelectMenu.DropDownItems[0].PerformClick();
        }

        public static void SendNotification(string text, string title = null)
        {
            _notifyIcon.BalloonTipTitle = GetParsedString(title);
            _notifyIcon.BalloonTipText = GetParsedString(text);
            _notifyIcon.ShowBalloonTip(2000);
        }

        private static string GetParsedString(string source)
        {
            var settings = SettingsController.GetSettingsInstance();

            foreach (var value in Enum.GetValues(typeof(GeneralSetting)).Cast<GeneralSetting>().ToList())
            {
                if (settings.GeneralSettings.ContainsKey(value))
                {
                    source = source.Replace(value.GetTemplate(), settings.GeneralSettings[value].ToString());
                }
            }

            foreach (var value in Enum.GetValues(typeof(KeybindSetting)).Cast<KeybindSetting>().ToList())
            {
                if (settings.KeybindSettings.ContainsKey(value))
                {
                    source = source.Replace(value.GetTemplate(), settings.KeybindSettings[value].ToString());
                }
            }

            return source;
        }

        public static void Dispose()
        {
            _notifyIcon?.Dispose();
        }
    }
}
