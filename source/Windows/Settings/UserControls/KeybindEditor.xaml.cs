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

        public static readonly DependencyProperty HotkeyProperty =
        DependencyProperty.Register(nameof(Hotkey), typeof(Hotkey), typeof(KeybindEditor), new FrameworkPropertyMetadata(default(Hotkey), FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));

        public Hotkey Hotkey
        {
            get => (Hotkey)GetValue(HotkeyProperty);
            set => SetValue(HotkeyProperty, value);
        }

        //private readonly KeyEventHandler previewKeydownHandler;

        private static IKeyboardMouseEvents _globalHook;
        public KeybindEditor()
        {
            InitializeComponent();
            //previewKeydownHandler = new KeyEventHandler(KeybindTextBox_PreviewKeyDown);
        }

        /// <summary>
        /// Captures the pressed hotkey
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        //private void KeybindTextBox_PreviewKeyDown(object sender, KeyEventArgs e)
        //{
        //    if (e.IsRepeat) return;

        //    //e.Handled = true;

        //    var modifiers = Keyboard.Modifiers;
        //    var key = e.Key;

        //    // When Alt is pressed, SystemKey is used instead
        //    if (key == Key.System)
        //    {
        //        key = e.SystemKey;
        //    }

        //    /*// Pressing delete, backspace without modifiers clears the current value
        //    if (modifiers == ModifierKeys.None &&
        //        (key == Key.Delete || key == Key.Back))
        //    {
        //        Hotkey = null;
        //        return;
        //    }*/

        //    // If no actual key was pressed - return
        //    if (key == Key.LeftCtrl ||
        //        key == Key.RightCtrl ||
        //        key == Key.LeftAlt ||
        //        key == Key.RightAlt ||
        //        key == Key.LeftShift ||
        //        key == Key.RightShift ||
        //        key == Key.LWin ||
        //        key == Key.RWin ||
        //        key == Key.Clear ||
        //        key == Key.OemClear ||
        //        key == Key.Apps)
        //    {
        //            return;
        //    }

        //    Hotkey = new Hotkey(key, modifiers);

        //    //stop listening to keyboard
        //    Keyboard.RemovePreviewKeyDownHandler(sender as Button, previewKeydownHandler);
        //    OnHotkeyChanged();
        //}
        private void KeybindTextBox_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            e.Handled = true;

            var modifiers = e.Modifiers;
            var key = e.KeyCode;

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

            Hotkey = new Hotkey(key, modifiers);

            //stop listening to keyboard
            _globalHook.KeyDown -= KeybindTextBox_PreviewKeyDown;
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

            _globalHook = Hook.GlobalEvents();
            _globalHook.KeyDown += KeybindTextBox_PreviewKeyDown;

            //Remove if there is a handler remaining
            //Keyboard.RemovePreviewKeyDownHandler(sender as Button, previewKeydownHandler);

            ////start listening to keyboard
            //Keyboard.AddPreviewKeyDownHandler(sender as Button, previewKeydownHandler);
        }
    }
}
