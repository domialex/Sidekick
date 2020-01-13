using Sidekick.Helpers;
//using System.Windows.Input;
using System.Windows.Forms;
using System.Linq;

namespace Sidekick.Windows.Settings.Models
{
    public enum GeneralSetting
    {
        None
    }

    public enum KeybindSetting
    {
        None,
        CloseWindow,
        PriceCheck,
        Hideout,
        ItemWiki
    }

    public class Settings
    {       
        public ObservableDictionary<GeneralSetting, string> GeneralSettings { get; set; } = new ObservableDictionary<GeneralSetting, string>();
        public ObservableDictionary<KeybindSetting, Hotkey> KeybindSettings { get; set; } = new ObservableDictionary<KeybindSetting, Hotkey>();

        public KeybindSetting GetKeybindSetting(Keys key, Keys modifier)
        {
            var value = KeybindSettings.Values.FirstOrDefault(x => x?.Key == key && x?.Modifiers == modifier);
            if (value != null)
            {
                return KeybindSettings.TryGetKey(value, out KeybindSetting setting) ? setting : KeybindSetting.None;
            }
            return KeybindSetting.None;
        }

        //Takes a winforms hotkey and returns the keybind setting
        //public Setting GetKeybindSetting(int winformsKey, int winformsModifier)
        //{
        //    Key key = KeyInterop.KeyFromVirtualKey(winformsKey);
        //    ModifierKeys modifiers = ModifierFromVirtualKey(winformsModifier);

        //    var value = KeybindSettings.Values.FirstOrDefault(x => x?.Key == key && x?.Modifiers == modifiers);
        //    if(value != null)
        //    {
        //        return KeybindSettings.TryGetKey(value, out Setting setting) ? setting : Setting.None;
        //    }
        //    return Setting.None;
        //}

        //private ModifierKeys ModifierFromVirtualKey(int winformsModifier)
        //{
        //    switch (winformsModifier)
        //    {
        //        case (int)System.Windows.Forms.Keys.Control: return ModifierKeys.Control;
        //        case (int)System.Windows.Forms.Keys.Shift: return ModifierKeys.Shift;
        //        case (int)System.Windows.Forms.Keys.Alt: return ModifierKeys.Alt;
        //        case (int)(System.Windows.Forms.Keys.Alt | System.Windows.Forms.Keys.Shift): return ModifierKeys.Alt | ModifierKeys.Shift;
        //        case (int)(System.Windows.Forms.Keys.Alt | System.Windows.Forms.Keys.Control): return ModifierKeys.Alt | ModifierKeys.Control;
        //        case (int)(System.Windows.Forms.Keys.Shift | System.Windows.Forms.Keys.Control): return ModifierKeys.Shift | ModifierKeys.Control;
        //        default: return ModifierKeys.None;
        //    }
        //}

        public void Clear()
        {
            GeneralSettings.Clear();
            KeybindSettings.Clear();
        }
    }
}
