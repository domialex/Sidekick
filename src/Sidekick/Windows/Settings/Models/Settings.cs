using Newtonsoft.Json;
using Sidekick.Business.Languages.UI;
using Sidekick.Core.Settings;
using Sidekick.Helpers;
using Sidekick.Helpers.POEDbAPI;
using Sidekick.Helpers.POEWikiAPI;
using System;
using System.Linq;
using WindowsHook;

namespace Sidekick.Windows.Settings.Models
{
    public class Settings
    {
        public ObservableDictionary<GeneralSetting, string> GeneralSettings { get; set; } = new ObservableDictionary<GeneralSetting, string>();
        public ObservableDictionary<KeybindSetting, Hotkey> KeybindSettings { get; set; } = new ObservableDictionary<KeybindSetting, Hotkey>();
        public WikiSetting CurrentWikiSettings { get; set; }
        [JsonIgnore]
        public IUILanguageProvider CurrentUILanguageProvider { get; set; } = Legacy.UILanguageProvider;
        public UILanguageAttribute CurrentUILanguage { get { return CurrentUILanguageProvider.Current; } set { CurrentUILanguageProvider.SetLanguage(value); } }

        public KeybindSetting GetKeybindSetting(Keys key, Keys modifier)
        {
            var value = KeybindSettings.Values.FirstOrDefault(x => x?.Key == key && x?.Modifiers == modifier);
            if (value != null)
            {
                return KeybindSettings.TryGetKey(value, out KeybindSetting setting) ? setting : KeybindSetting.None;
            }
            return KeybindSetting.None;
        }

        public Action<Sidekick.Business.Parsers.Models.Item> GetWikiAction()
        {
            if (CurrentWikiSettings == WikiSetting.PoeWiki)
            {
                return POEWikiHelper.Open;
            }
            else if (CurrentWikiSettings == WikiSetting.PoeDb)
            {
                return POEDbClient.Open;
            }

            return null;
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
