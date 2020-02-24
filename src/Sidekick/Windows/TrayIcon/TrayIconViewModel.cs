using System.Windows.Input;
using Sidekick.UI.Views;
using Sidekick.Windows.ApplicationLogs;
using Sidekick.Windows.Settings;

namespace Sidekick.Windows.TrayIcon
{
    public class TrayIconViewModel
    {
        private readonly App application;
        private readonly IViewLocator viewLocator;

        public TrayIconViewModel(
            App application,
            IViewLocator viewLocator)
        {
            this.application = application;
            this.viewLocator = viewLocator;
        }

        public ICommand ShowSettingsCommand => new RelayCommand(_ => viewLocator.Open<SettingsView>());

        public ICommand ShowLogsCommand => new RelayCommand(_ => viewLocator.Open<ApplicationLogsView>());

        public ICommand ExitApplicationCommand => new RelayCommand(_ => application.Shutdown());
    }
}
