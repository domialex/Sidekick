using Newtonsoft.Json;
using Sidekick.Business.Languages;
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
        public UILanguageEnum CurrentUILanguage { get { return CurrentUILanguageProvider.Current; } set { CurrentUILanguageProvider.SetUILanguageProvider(value); } }

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

        public void Clear()
        {
            GeneralSettings.Clear();
            KeybindSettings.Clear();
        }
    }
}
