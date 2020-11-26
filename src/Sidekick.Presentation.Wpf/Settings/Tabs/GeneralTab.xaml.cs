using System.Windows;
using System.Windows.Controls;
using Bindables;

namespace Sidekick.Presentation.Wpf.Settings.Tabs
{
    /// <summary>
    /// Interaction logic for GeneralTab.xaml
    /// </summary>
    [DependencyProperty]
    public partial class GeneralTab : UserControl
    {
        public GeneralTab()
        {
            InitializeComponent();
            ScrollViewer.DataContext = this;
        }

        public SettingsViewModel ViewModel { get; set; }
    }
}
