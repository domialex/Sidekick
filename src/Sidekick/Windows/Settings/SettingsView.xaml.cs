using System.Windows;
using System.Windows.Forms.Integration;
using System.Windows.Input;
using Sidekick.UI.Settings;
using Sidekick.UI.Views;

namespace Sidekick.Windows.Settings
{
    public partial class SettingsView : Window, ISidekickView
    {
        private const int WINDOW_WIDTH = 480;
        private const int WINDOW_HEIGHT = 320;
        private readonly ISettingsViewModel viewModel;

        public SettingsView(ISettingsViewModel viewModel)
        {
            this.viewModel = viewModel;

            Width = WINDOW_WIDTH;
            Height = WINDOW_HEIGHT;

            InitializeComponent();
            DataContext = viewModel;

            ElementHost.EnableModelessKeyboardInterop(this);

            Show();
        }

        private void Window_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            DragMove();
        }

        private void SaveChanges_Click(object sender, RoutedEventArgs e)
        {
            viewModel.Save();
            Close();
        }

        private void DiscardChanges_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
