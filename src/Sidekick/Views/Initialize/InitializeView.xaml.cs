using System.Windows;
using Sidekick.Core.Settings;
using Sidekick.Views.ApplicationLogs;

namespace Sidekick.Views.Initialize
{
    /// <summary>
    /// Interaction logic for SplashScreen.xaml
    /// </summary>
    public partial class InitializeView : Window, ISidekickView
    {
        private readonly InitializeViewModel viewModel;
        private readonly IViewLocator viewLocator;

        public InitializeView(InitializeViewModel viewModel,
            IViewLocator viewLocator,
            SidekickSettings settings)
        {
            this.viewModel = viewModel;
            this.viewLocator = viewLocator;

            InitializeComponent();
            DataContext = viewModel;

            viewModel.Initialized += SplashViewModel_Initialized;

            if (settings.ShowSplashScreen)
            {
                Show();
            }

            _ = viewModel.Initialize();
        }

        private void SplashViewModel_Initialized()
        {
            viewModel.Initialized -= SplashViewModel_Initialized;
            Dispatcher.Invoke(Close);
        }

        private void Close_Click(object sender, RoutedEventArgs e)
        {
            viewModel.Close();
        }

        private void Logs_Click(object sender, RoutedEventArgs e)
        {
            viewLocator.Open<ApplicationLogsView>();
        }
    }
}
