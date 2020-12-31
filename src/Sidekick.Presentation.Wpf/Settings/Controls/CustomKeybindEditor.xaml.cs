using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Bindables;
using Sidekick.Domain.Settings;

namespace Sidekick.Presentation.Wpf.Settings.Controls
{
    /// <summary>
    /// Interaction logic for CustomKeybindEditor.xaml
    /// </summary>
    [DependencyProperty]
    public partial class CustomKeybindEditor : UserControl
    {
        public CustomChatModel CustomChat { get; set; }

        public SettingsViewModel ViewModel { get; set; }

        public SolidColorBrush BackgroundColor { get; private set; }

        public CustomKeybindEditor()
        {
            InitializeComponent();

            Grid.DataContext = this;
            Loaded += CustomKeybindEditor_Loaded;
            Unloaded += CustomKeybindEditor_Unloaded;
        }

        private void CustomKeybindEditor_Loaded(object sender, RoutedEventArgs e)
        {
            if (ViewModel != null)
                ViewModel.PropertyChanged += ViewModel_PropertyChanged;
        }

        private void CustomKeybindEditor_Unloaded(object sender, RoutedEventArgs e)
        {
            ViewModel.SettingCustom = false;
        }

        private void ViewModel_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(ViewModel.SettingCustom))
            {
                if (ViewModel != null && ViewModel.SettingCustom && CustomChat == ViewModel.CurrentCustomChat)
                {
                    BackgroundColor = new SolidColorBrush(Color.FromRgb(200, 200, 200));
                }
                else
                {
                    BackgroundColor = new SolidColorBrush(Color.FromArgb(0, 200, 200, 200));
                }
            }
        }

        private void HotkeyButton_Click(object sender, RoutedEventArgs e)
        {
            ViewModel.CurrentCustomChat = CustomChat;
            ViewModel.SettingCustom = true;
        }

        private void ClearButton_Click(object sender, RoutedEventArgs e)
        {
            ViewModel.CustomChatSettings.Remove(CustomChat);
        }
    }
}
