using System.Windows;
using System.Windows.Controls;
using Bindables;

namespace Sidekick.Presentation.Wpf.Settings.Tabs
{
    /// <summary>
    /// Interaction logic for ChatTab.xaml
    /// </summary>
    [DependencyProperty]
    public partial class ChatTab : UserControl
    {
        public ChatTab()
        {
            InitializeComponent();
            ScrollViewer.DataContext = this;
        }

        public SettingsViewModel ViewModel { get; set; }
    }
}
