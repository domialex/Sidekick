using System.Globalization;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using Sidekick.Business.Leagues;
using Sidekick.Core.Initialization;
using Sidekick.Core.Settings;
using Sidekick.Localization;
using Sidekick.Localization.Tray;
using Sidekick.UI.Helpers;
using Sidekick.Windows.ApplicationLogs;
using Sidekick.Windows.Settings;
using Sidekick.Windows.TrayIcon.Models;

namespace Sidekick.Windows.TrayIcon
{
    public interface ITrayIconViewModel
    {
        string SettingsHeader { get; set; }
        string ShowLogsHeader { get; set; }
        string ExitHeader { get; set; }

    }

    public class TrayIconViewModel : NotifyBase, ITrayIconViewModel, IOnAfterInit
    {
        #region Property backing fields

        private string settingsHeader;
        private string showLogsHeader;
        private string exitHeader;

        #endregion

        public string SettingsHeader { get => settingsHeader; set => NotifyProperty(ref settingsHeader, value); }
        public string ShowLogsHeader { get => showLogsHeader; set => NotifyProperty(ref showLogsHeader, value); }
        public string ExitHeader { get => exitHeader; set => NotifyProperty(ref exitHeader, value); }

        public AsyncObservableCollection<League> Leagues { get; private set; }

        private readonly SidekickSettings configuration;
        private readonly IUILanguageProvider uiLanguageProvider;
        private readonly ILeagueService leagueService;

        public TrayIconViewModel(SidekickSettings configuration, IUILanguageProvider uiLanguageProvider, ILeagueService leagueService)
        {
            this.configuration = configuration;
            this.uiLanguageProvider = uiLanguageProvider;
            this.leagueService = leagueService;

            uiLanguageProvider.UILanguageChanged += UpdateLanguage;
            UpdateLanguage();
            Leagues = new AsyncObservableCollection<League>();
        }

        private void UpdateLanguage()
        {
            var cultureInfo = new CultureInfo(uiLanguageProvider.Current.Name);
            Thread.CurrentThread.CurrentCulture = cultureInfo;
            Thread.CurrentThread.CurrentUICulture = cultureInfo;

            SettingsHeader = TrayResources.Settings;
            ShowLogsHeader = TrayResources.ShowLogs;
            ExitHeader = TrayResources.Exit;
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

        public Task OnAfterInit()
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

            return Task.CompletedTask;
        }

        public ICommand ChangeLeagueCommand => new RelayCommand(leagueId => ChangeLeague(leagueId.ToString()));

        public ICommand ShowSettingsCommand => new RelayCommand(_ => Legacy.ViewLocator.Open<SettingsView>());

        public ICommand ShowLogsCommand => new RelayCommand(_ => ApplicationLogsController.Show());

        public ICommand ExitApplicationCommand => new RelayCommand(_ => Application.Current.Shutdown());
    }
}
