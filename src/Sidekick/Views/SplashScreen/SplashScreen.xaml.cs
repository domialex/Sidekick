using System.Windows;
using Sidekick.UI.Splash;
using Sidekick.UI.Views;
using Sidekick.Views.ApplicationLogs;

namespace Sidekick.Views
{
    /// <summary>
    /// Interaction logic for SplashScreen.xaml
    /// </summary>
    public partial class SplashScreen : Window, ISidekickView
    {
        private readonly ISplashViewModel viewModel;
        private readonly IViewLocator viewLocator;

        public SplashScreen(ISplashViewModel viewModel, IViewLocator viewLocator)
        {
            InitializeComponent();
            DataContext = viewModel;

            viewModel.Initialized += SplashViewModel_Initialized;
            Show();
            this.viewModel = viewModel;
            this.viewLocator = viewLocator;
        }

        private void SplashViewModel_Initialized()
        {
            viewModel.Initialized -= SplashViewModel_Initialized;
            Dispatcher.Invoke(Close);
        }

        private void Close_Click(object sender, RoutedEventArgs e)
        {
            App.Instance.Shutdown();
        }

        private void Logs_Click(object sender, RoutedEventArgs e)
        {
            viewLocator.Open<ApplicationLogsView>();
        }
    }
}
