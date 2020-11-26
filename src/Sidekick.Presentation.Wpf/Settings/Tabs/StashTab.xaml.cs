using System.Windows;
using System.Windows.Controls;
using Bindables;

namespace Sidekick.Presentation.Wpf.Settings.Tabs
{
    /// <summary>
    /// Interaction logic for StashTab.xaml
    /// </summary>
    [DependencyProperty]
    public partial class StashTab : UserControl
    {
        public StashTab()
        {
            InitializeComponent();
            ScrollViewer.DataContext = this;
        }

        public SettingsViewModel ViewModel { get; set; }
    }
}
