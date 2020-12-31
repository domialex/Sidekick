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

        private void NewCommand_Click(object sender, RoutedEventArgs e)
        {
            ViewModel.NewCommand();
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            icCustomCommands.ItemsSource = ViewModel.Custom_Chat_Settings;
        }
    }
}
