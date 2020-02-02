using System.Windows;
using Sidekick.Localization.Splash;
using Sidekick.UI.Splash;
using Sidekick.UI.Views;

namespace Sidekick.Windows
{
    /// <summary>
    /// Interaction logic for SplashScreen.xaml
    /// </summary>
    public partial class SplashScreen : Window, ISidekickView
    {
        private readonly ISplashViewModel splashViewModel;

        public SplashScreen(ISplashViewModel splashViewModel)
        {
            InitializeComponent();
            DataContext = splashViewModel;

            splashViewModel.Initialized += SplashViewModel_Initialized;
            Show();
            this.splashViewModel = splashViewModel;
        }

        private void SplashViewModel_Initialized()
        {
            splashViewModel.Initialized -= SplashViewModel_Initialized;
            Dispatcher.Invoke(Close);
        }
    }
}
