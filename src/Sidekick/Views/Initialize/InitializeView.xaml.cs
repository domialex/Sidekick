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
        private readonly SidekickSettings settings;

        public InitializeView(InitializeViewModel viewModel,
            IViewLocator viewLocator,
            SidekickSettings settings)
        {
            this.viewModel = viewModel;
            this.viewLocator = viewLocator;
            this.settings = settings;
            InitializeComponent();
            DataContext = viewModel;

            viewModel.Initialized += SplashViewModel_Initialized;

            if (settings.ShowSplashScreen)
            {
                Show();
            }
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
