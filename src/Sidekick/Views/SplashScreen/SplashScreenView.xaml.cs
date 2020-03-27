using System.Windows;
using Sidekick.Views.ApplicationLogs;

namespace Sidekick.Views.SplashScreen
{
    /// <summary>
    /// Interaction logic for SplashScreen.xaml
    /// </summary>
    public partial class SplashScreenView : Window, ISidekickView
    {
        private readonly SplashViewModel viewModel;
        private readonly IViewLocator viewLocator;

        public SplashScreenView(SplashViewModel viewModel, IViewLocator viewLocator)
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
