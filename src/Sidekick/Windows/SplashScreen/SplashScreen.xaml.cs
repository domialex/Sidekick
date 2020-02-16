using System.Windows;
using Sidekick.UI.Splash;
using Sidekick.UI.Views;

namespace Sidekick.Windows
{
    /// <summary>
    /// Interaction logic for SplashScreen.xaml
    /// </summary>
    public partial class SplashScreen : Window, ISidekickView
    {
        private readonly ISplashViewModel viewModel;

        public SplashScreen(ISplashViewModel viewModel)
        {
            InitializeComponent();
            DataContext = viewModel;

            viewModel.Initialized += SplashViewModel_Initialized;
            Show();
            this.viewModel = viewModel;
        }

        private void SplashViewModel_Initialized()
        {
            viewModel.Initialized -= SplashViewModel_Initialized;
            Dispatcher.Invoke(Close);
        }
    }
}
