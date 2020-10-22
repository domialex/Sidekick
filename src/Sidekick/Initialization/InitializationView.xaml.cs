using System.Windows;
using Sidekick.Views;
using Sidekick.Views.ApplicationLogs;

namespace Sidekick.Initialization
{
    /// <summary>
    /// Interaction logic for SplashScreen.xaml
    /// </summary>
    public partial class InitializationView : Window, ISidekickView
    {
        private readonly InitializationViewModel viewModel;
        private readonly IViewLocator viewLocator;

        public InitializationView(
            InitializationViewModel viewModel,
            IViewLocator viewLocator)
        {
            this.viewModel = viewModel;
            this.viewLocator = viewLocator;

            InitializeComponent();
            DataContext = viewModel;
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
