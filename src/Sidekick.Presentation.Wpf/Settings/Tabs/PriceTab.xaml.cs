using System.Windows;
using System.Windows.Controls;
using Bindables;

namespace Sidekick.Presentation.Wpf.Settings.Tabs
{
    /// <summary>
    /// Interaction logic for PriceTab.xaml
    /// </summary>
    [DependencyProperty]
    public partial class PriceTab : UserControl
    {
        public PriceTab()
        {
            InitializeComponent();
            ScrollViewer.DataContext = this;
        }

        public SettingsViewModel ViewModel { get; set; }
    }
}
