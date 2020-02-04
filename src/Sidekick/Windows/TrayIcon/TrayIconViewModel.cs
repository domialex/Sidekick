using System.Globalization;
using System.Threading;
using System.Windows;
using System.Windows.Input;
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

    public class TrayIconViewModel : NotifyBase, ITrayIconViewModel
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

        public TrayIconViewModel(SidekickSettings configuration, IUILanguageProvider uiLanguageProvider)
        {
            this.configuration = configuration;
            this.uiLanguageProvider = uiLanguageProvider;

            uiLanguageProvider.UILanguageChanged += UpdateLanguage;
            UpdateLanguage();
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

        public ICommand ShowSettingsCommand => new RelayCommand(_ => Legacy.ViewLocator.Open<SettingsView>());

        public ICommand ShowLogsCommand => new RelayCommand(_ => ApplicationLogsController.Show());

        public ICommand ExitApplicationCommand => new RelayCommand(_ => Application.Current.Shutdown());
    }
}
