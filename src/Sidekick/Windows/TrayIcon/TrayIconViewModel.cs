using System.Windows.Input;
using System.Windows;
using Sidekick.Business.Leagues;
using Sidekick.Windows.Settings;
using Sidekick.Windows.ApplicationLogs;
using Sidekick.Business.Languages.UI;
using System.Threading.Tasks;

namespace Sidekick.Windows.TrayIcon
{
    public class TrayIconViewModel : NotifyBase
    {
        #region Property backing fields

        private string settingsHeader;
        private string showLogsHeader;
        private string exitHeader;

        #endregion

        public string SettingsHeader { get => settingsHeader; set => NotifyProperty(ref settingsHeader, value); }
        public string ShowLogsHeader { get => showLogsHeader; set => NotifyProperty(ref showLogsHeader, value); }
        public string ExitHeader { get => exitHeader; set => NotifyProperty(ref exitHeader, value); }

        private readonly IUILanguageProvider uiLanguageProvider;
        private readonly ILeagueService leagueService;

        public TrayIconViewModel(IUILanguageProvider uiLanguageProvider, ILeagueService leagueService)
        {
            this.uiLanguageProvider = uiLanguageProvider;
            this.leagueService = leagueService;

            uiLanguageProvider.UILanguageChanged += UpdateLanguage;
            UpdateLanguage();
        }

        private void UpdateLanguage()
        {
            SettingsHeader = uiLanguageProvider.Language.TrayIconSettings;
            ShowLogsHeader = uiLanguageProvider.Language.TrayIconShowLogs;
            ExitHeader = uiLanguageProvider.Language.TrayIconExit;
        }

        public ICommand ShowSettingsCommand => new DelegateCommand
        {
            CommandAction = () => SettingsController.Show()
        };

        public ICommand ShowLogsCommand => new DelegateCommand
        {
            CommandAction = () => ApplicationLogsController.Show()
        };

        public ICommand ExitApplicationCommand => new DelegateCommand
        {
            CommandAction = () => Application.Current.Shutdown()
        };
    }
}
