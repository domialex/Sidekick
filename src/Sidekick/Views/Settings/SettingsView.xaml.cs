using System;
using System.Windows;

namespace Sidekick.Views.Settings
{
    public partial class SettingsView : BaseWindow
    {
        private readonly SettingsViewModel viewModel;

        public SettingsView(IServiceProvider serviceProvider, SettingsViewModel viewModel)
            : base("settings", serviceProvider)
        {
            this.viewModel = viewModel;

            InitializeComponent();
            DataContext = viewModel;

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
    }
}
