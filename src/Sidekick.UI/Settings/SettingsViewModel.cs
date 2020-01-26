using System.Collections.Generic;
using System.Linq;
using Sidekick.Business.Languages.UI;
using Sidekick.Core.Settings;

namespace Sidekick.UI.Settings
{
    public class SettingsViewModel : ISettingsViewModel
    {
        private readonly IUILanguageProvider uiLanguageProvider;
        private readonly SidekickSettings sidekickSettings;

        public SettingsViewModel(IUILanguageProvider uiLanguageProvider,
            SidekickSettings sidekickSettings)
        {
            this.uiLanguageProvider = uiLanguageProvider;
            this.sidekickSettings = sidekickSettings;

            Settings = new SidekickSettings();
            AssignValues(sidekickSettings, Settings);

            Keybinds.Clear();
            Settings.GetType()
                .GetProperties()
                .Where(x => x.Name.StartsWith("Key"))
                .ToList()
                .ForEach(x =>
                {
                    Keybinds.Add(x.Name, x.GetValue(Settings).ToString());
                });
        }

        public SidekickSettings Settings { get; private set; }

        public Dictionary<string, string> Keybinds { get; private set; } = new Dictionary<string, string>();

        public void Save()
        {
            AssignValues(Settings, sidekickSettings);
            uiLanguageProvider.SetLanguage(uiLanguageProvider.AvailableLanguages.First(x => x.Name == Settings.UILanguage));
            sidekickSettings.Save();
        }

        public bool IsKeybindUsed(string keybind, string ignoreKey = null)
        {
            return Keybinds.Any(x => x.Value == keybind && x.Key != ignoreKey);
        }

        private static void AssignValues(SidekickSettings src, SidekickSettings dest)
        {
            // iterates through src Settings (properties) and copies them to dest settings (properties)
            // If there ever comes a time, where some properties do not have to be copied, we can add attributes to exclude them
            src.GetType().GetProperties().ToList().ForEach(x => dest.GetType().GetProperty(x.Name).SetValue(dest, x.GetValue(src))); 
        }
    }
}
