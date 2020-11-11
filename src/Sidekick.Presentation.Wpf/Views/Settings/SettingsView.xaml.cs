using System;
using System.Windows;

namespace Sidekick.Presentation.Wpf.Views.Settings
{
    public partial class SettingsView : BaseView
    {
        private readonly SettingsViewModel viewModel;

        public SettingsView(IServiceProvider serviceProvider, SettingsViewModel viewModel)
            : base(Domain.Views.View.Settings, serviceProvider)
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

            _ = viewModel.Initialize();
        }

        private async void SaveChanges_Click(object sender, RoutedEventArgs e)
        {
            Hide();
            await viewModel.Save();
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
