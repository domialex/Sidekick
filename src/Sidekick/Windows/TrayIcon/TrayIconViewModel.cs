using System.Windows.Input;
using System.Windows;
using Sidekick.Business.Leagues;
using Sidekick.Windows.Settings;
using Sidekick.Windows.ApplicationLogs;
using Sidekick.Business.Languages.UI;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using Sidekick.Windows.TrayIcon.Models;
using System.Linq;
using Sidekick.Core.Configuration;

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

        public ObservableCollection<League> Leagues { get; private set; }

        private readonly Configuration configuration;
        private readonly IUILanguageProvider uiLanguageProvider;
        private readonly ILeagueService leagueService;

        public TrayIconViewModel(Configuration configuration, IUILanguageProvider uiLanguageProvider, ILeagueService leagueService)
        {
            this.configuration = configuration;
            this.uiLanguageProvider = uiLanguageProvider;
            this.leagueService = leagueService;

            uiLanguageProvider.UILanguageChanged += UpdateLanguage;
            leagueService.LeaguesUpdated += UpdateLeagues;
            UpdateLanguage();
            Leagues = new ObservableCollection<League>();
        }

        private void UpdateLanguage()
        {
            SettingsHeader = uiLanguageProvider.Language.TrayIconSettings;
            ShowLogsHeader = uiLanguageProvider.Language.TrayIconShowLogs;
            ExitHeader = uiLanguageProvider.Language.TrayIconExit;
        }

        private void UpdateLeagues()
        {
            var leagues = leagueService.Leagues.Select(l => new League
            {
                Id = l.Id,
                Name = l.Text,
                IsCurrent = l.Id == configuration.LeagueId
            });

            Leagues.Clear();

            foreach (var league in leagues)
            {
                Leagues.Add(league);
            }
        }

        private void ChangeLeague(string id)
        {
            foreach (var league in Leagues)
            {
                league.IsCurrent = league.Id == id;
            }

            configuration.LeagueId = id;
            configuration.Save();
        }

        public ICommand ChangeLeagueCommand => new RelayCommand(leagueId => ChangeLeague(leagueId.ToString()));

        public ICommand ShowSettingsCommand => new RelayCommand(_ => SettingsController.Show());

        public ICommand ShowLogsCommand => new RelayCommand(_ => ApplicationLogsController.Show());

        public ICommand ExitApplicationCommand => new RelayCommand(_ => Application.Current.Shutdown());
    }
}
