using Sidekick.Core.Settings;
using Sidekick.Windows.Settings.UserControls;
using System;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using WindowsHook;

namespace Sidekick.Windows.Settings
{
    /// <summary>
    /// Interaction logic for UserControl1.xaml
    /// </summary>
    public partial class SettingsView : Window
    {
        public event EventHandler OnWindowClosed;

        public bool IsDisplayed => Visibility == Visibility.Visible;

        public Models.Settings Settings { get; set; }

        public SettingsView(int width, int height)
        {
            Width = width;
            Height = height;

            InitializeComponent();
            DataContext = this;

            Settings = SettingsController.LoadSettings();
            SelectWikiSetting();
            Show();
        }

        private void Window_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            DragMove();
        }

        private void SelectWikiSetting()
        {
            if (Settings.CurrentWikiSettings == WikiSetting.PoeWiki)
            {
                radioButtonPOEWiki.IsChecked = true;
            }
            else if (Settings.CurrentWikiSettings == WikiSetting.PoeDb)
            {
                radioButtonPOEDb.IsChecked = true;
            }
        }

        private void UpdateWikiSetting()
        {
            if (radioButtonPOEWiki.IsChecked == true)
            {
                Settings.CurrentWikiSettings = WikiSetting.PoeWiki;
            }
            else if (radioButtonPOEDb.IsChecked == true)
            {
                Settings.CurrentWikiSettings = WikiSetting.PoeDb;
            }
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            OnWindowClosed?.Invoke(this, null);
            base.OnClosing(e);
        }

        private void SaveChanges_Click(object sender, RoutedEventArgs e)
        {
            UpdateWikiSetting();
            SettingsController.SaveSettings();
            Close();
        }

        private void DiscardChanges_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        public void CaptureKeyEvents(Keys key, Keys modifier)
        {
            if (currentChangingKeybind != null)
            {
                currentChangingKeybind.CaptureKeybinding(key, modifier);
            }
        }

        /// <summary>
        /// Gets called after a new hotkey has been defined. Checks if that new hotkey is unique
        /// </summary>
        /// <param name="sender"></param>
        private void KeybindEditor_HotkeyChanged(object sender)
        {
            var control = sender as KeybindEditor;
            try
            {
                //Check if hotkeys are unique
                if (Settings.KeybindSettings.Values.Where(v => v?.ToString() == control.Hotkey?.ToString()).ToList().Count > 1)
                {
                    if (MessageBox.Show("Hotkey already in use!") == MessageBoxResult.OK)
                    {
                        control.Hotkey = null;
                    }
                }
                currentChangingKeybind = null;
            }
            catch (Exception)
            {
                Legacy.Logger.Log("Could not validate if Hotkey is already in use");
                control.Hotkey = null;
                throw;
            }
        }

        private KeybindEditor currentChangingKeybind;
        private void KeybindEditor_HotkeyChanging(object sender)
        {
            currentChangingKeybind = sender as KeybindEditor;
        }
    }
}
