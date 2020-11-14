using System.Windows;
using System.Windows.Controls;
using Bindables;

namespace Sidekick.Presentation.Wpf.Settings.Tabs
{
    /// <summary>
    /// Interaction logic for WikiTab.xaml
    /// </summary>
    [DependencyProperty]
    public partial class WikiTab : UserControl
    {
        public WikiTab()
        {
            InitializeComponent();
            ScrollViewer.DataContext = this;
        }

        public SettingsViewModel ViewModel { get; set; }
    }
}
