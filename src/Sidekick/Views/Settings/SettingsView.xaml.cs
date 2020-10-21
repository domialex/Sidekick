using System;
using System.Windows;

namespace Sidekick.Views.Settings
{
    public partial class SettingsView : BaseView
    {
        private readonly SettingsViewModel viewModel;

        public SettingsView(IServiceProvider serviceProvider, SettingsViewModel viewModel)
            : base("settings", serviceProvider)
        {
            this.viewModel = viewModel;

            InitializeComponent();
            DataContext = viewModel;

            Topmost = true;
            Dispatcher.Invoke(async () =>
            {
                await System.Threading.Tasks.Task.Delay(1000);
                Topmost = false;
            });

            Show();
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

        private async void ResetCache_Click(object sender, RoutedEventArgs e)
        {
            Hide();
            await viewModel.ResetCache();
            Close();
        }
    }
}
