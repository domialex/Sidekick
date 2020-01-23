using Sidekick.Core.Settings;
using Sidekick.Windows.ApplicationLogs;
using Sidekick.Windows.Settings;
using System;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Sidekick.Helpers
{
    public static class TrayIcon
    {
        private static NotifyIcon NotifyIcon;

        public static void Initialize()
        {
            NotifyIcon = new NotifyIcon();
            var icon = Resources.ExaltedOrb;
            NotifyIcon.Icon = icon;
            NotifyIcon.Visible = true;
            NotifyIcon.Text = "Sidekick";

            ReloadUI();

            Legacy.UILanguageProvider.UILanguageChanged += ReloadUI;
        }

        public static void ReloadUI()
        {
            var settings = SettingsController.GetSettingsInstance();
            var contextMenu = new ContextMenuStrip();

            // League selection
            if (Legacy.LeagueService.Leagues != null)
            {
                var leagueMenu = new ToolStripMenuItem("League");
                foreach (var league in Legacy.LeagueService.Leagues)
                {
                    var menuItem = new ToolStripMenuItem(league.Id);
                    menuItem.Checked = league.Id == Legacy.Configuration.LeagueId;
                    menuItem.Click += (s, e) =>
                    {
                        foreach (ToolStripMenuItem x in leagueMenu.DropDownItems)
                        {
                            x.Checked = false;
                        }
                        menuItem.Checked = true;
                        Legacy.Configuration.LeagueId = league.Id;
                        Legacy.Configuration.Save();
                    };
                    leagueMenu.DropDownItems.Add(menuItem);
                }
                contextMenu.Items.Add(leagueMenu);
            }

            // Separator
            contextMenu.Items.Add(new ToolStripSeparator());

            // Settings button
            contextMenu.Items.Add(settings.CurrentUILanguageProvider.Language.TrayIconSettings, null, (s, e) => SettingsController.Show());

            // Logs button
            contextMenu.Items.Add(settings.CurrentUILanguageProvider.Language.TrayIconShowLogs, null, (s, e) => ApplicationLogsController.Show());

            // Exit button
            contextMenu.Items.Add(settings.CurrentUILanguageProvider.Language.TrayIconExit, null, (s, e) => Application.Exit());

            NotifyIcon.ContextMenuStrip = contextMenu;
        }

        public static void SendNotification(string text, string title = null)
        {
            NotifyIcon.BalloonTipTitle = GetParsedString(title);
            NotifyIcon.BalloonTipText = GetParsedString(text);
            NotifyIcon.ShowBalloonTip(2000);
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
            NotifyIcon?.Dispose();
        }
    }
}
