using Sidekick.Business.Leagues;
using Sidekick.Business.Platforms;
using Sidekick.Business.Trades;
using Sidekick.Core.DependencyInjection.Services;
using Sidekick.Core.Settings;
using Sidekick.Windows.ApplicationLogs;
using Sidekick.Windows.Settings;
using System;
using System.Linq;
using System.Windows.Forms;

namespace Sidekick.Services
{
    [SidekickService(typeof(IPlatformTrayService))]
    public class WindowsTrayService : IPlatformTrayService, IDisposable
    {
        private readonly ITradeClient tradeClient;
        private readonly ILeagueService leagueService;

        public WindowsTrayService(ITradeClient tradeClient,
            ILeagueService leagueService)
        {
            this.tradeClient = tradeClient;
            this.leagueService = leagueService;

            _notifyIcon = new NotifyIcon();
            var icon = Resources.ExaltedOrb;
            _notifyIcon.Icon = icon;
            _notifyIcon.Visible = true;
            _notifyIcon.Text = "Sidekick";

            ReloadUI();

            SettingsController.GetSettingsInstance().CurrentUILanguageProvider.UILanguageChanged.Add(ReloadUI);
        }

        private void ReloadUI()
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

        private NotifyIcon _notifyIcon;
        private ToolStripMenuItem _leagueSelectMenu;

        public void SendNotification(string title, string text)
        {
            _notifyIcon.BalloonTipTitle = GetParsedString(title);
            _notifyIcon.BalloonTipText = GetParsedString(text);
            _notifyIcon.ShowBalloonTip(2000);
        }

        public void UpdateLeagues()
        {
            if (leagueService.Leagues == null)
            {
                return;
            }

            if (_leagueSelectMenu.DropDownItems.Count > 0)
            {
                // TODO: Fix Cross-thread operation not valid after changing language.
                leagueService.SelectedLeague = leagueService.Leagues.FirstOrDefault();
                return;
            }

            foreach (var league in leagueService.Leagues)
            {
                var menuItem = new ToolStripMenuItem(league.Id);
                menuItem.Click += (s, e) => { foreach (ToolStripMenuItem x in _leagueSelectMenu.DropDownItems) { x.Checked = false; } };
                menuItem.Click += (s, e) => { menuItem.Checked = true; };
                menuItem.Click += (s, e) => { leagueService.SelectedLeague = league; };
                _leagueSelectMenu.DropDownItems.Add(menuItem);
            }

            // Select the first league as the default.
            _leagueSelectMenu.DropDownItems[0].PerformClick();
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

        public void Dispose()
        {
            _notifyIcon?.Dispose();
        }
    }
}
