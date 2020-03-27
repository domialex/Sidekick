using System;
using System.Windows;
using Sidekick.UI.Settings;

namespace Sidekick.Views.Settings
{
    public partial class SettingsView : BaseWindow
    {
        private readonly ISettingsViewModel viewModel;

        public SettingsView(
            IServiceProvider serviceProvider,
            ISettingsViewModel viewModel)
            : base(serviceProvider)
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
