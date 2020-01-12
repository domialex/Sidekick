using Sidekick.Windows.Settings.Models;
using System.Windows;
using System.Windows.Controls;
//using System.Windows.Input;
using System.Windows.Forms;
using Gma.System.MouseKeyHook;

namespace Sidekick.Windows.Settings.UserControls
{
    /// <summary>
    /// Interaction logic for KeybindEditor.xaml
    /// </summary>
    public partial class KeybindEditor : System.Windows.Controls.UserControl
    {
        public delegate void HotkeyChangedEventHandler(object sender);
        public event HotkeyChangedEventHandler HotkeyChanged;
        public void OnHotkeyChanged()
        {
            HotkeyChanged?.Invoke(this);
        }

        public delegate void HotkeyChangingEventHandler(object sender);
        public event HotkeyChangingEventHandler HotkeyChanging;
        public void OnHotkeyChanging()
        {
            HotkeyChanging?.Invoke(this);
        }

        public static readonly DependencyProperty HotkeyProperty =
        DependencyProperty.Register(nameof(Hotkey), typeof(Hotkey), typeof(KeybindEditor), new FrameworkPropertyMetadata(default(Hotkey), FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));

        public Hotkey Hotkey
        {
            get => (Hotkey)GetValue(HotkeyProperty);
            set => SetValue(HotkeyProperty, value);
        }

        public KeybindEditor()
        {
            InitializeComponent();
        }

        public void CaptureKeybinding(System.Windows.Forms.Keys key, System.Windows.Forms.Keys modifier)
        {
            // If no actual key was pressed - return
            if (key == Keys.LControlKey ||
                key == Keys.RControlKey ||
                key == Keys.LMenu ||
                key == Keys.RMenu ||
                key == Keys.LShiftKey ||
                key == Keys.RShiftKey ||
                key == Keys.LWin ||
                key == Keys.RWin ||
                key == Keys.Clear ||
                key == Keys.OemClear ||
                key == Keys.Apps)
            {
                return;
            }

            Hotkey = new Hotkey(key, modifier);

            OnHotkeyChanged();
        }

        /// <summary>
        /// Starts to capture keyboard input when the button is pressed
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void HotkeyButton_PreviewMouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            this.Hotkey = null;
            OnHotkeyChanging();
        }
    }
}
