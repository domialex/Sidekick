using System.Windows;
using System.Windows.Input;
using Sidekick.UI.Settings;
using Sidekick.UI.Views;

namespace Sidekick.Windows.Settings
{
    public partial class SettingsView : Window, ISidekickView
    {
        private readonly ISettingsViewModel viewModel;

        public SettingsView(ISettingsViewModel viewModel)
        {
            this.viewModel = viewModel;

            InitializeComponent();
            DataContext = viewModel;

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
