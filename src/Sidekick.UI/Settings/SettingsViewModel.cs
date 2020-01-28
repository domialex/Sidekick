using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using Sidekick.Business.Languages.UI;
using Sidekick.Core.Natives;
using Sidekick.Core.Settings;

namespace Sidekick.UI.Settings
{
    public class SettingsViewModel : ISettingsViewModel, IDisposable
    {
        private readonly IUILanguageProvider uiLanguageProvider;
        private readonly SidekickSettings sidekickSettings;
        private readonly INativeKeyboard nativeKeyboard;

        public event PropertyChangedEventHandler PropertyChanged;

        public SettingsViewModel(IUILanguageProvider uiLanguageProvider,
            SidekickSettings sidekickSettings,
            INativeKeyboard nativeKeyboard)
        {
            this.uiLanguageProvider = uiLanguageProvider;
            this.sidekickSettings = sidekickSettings;
            this.nativeKeyboard = nativeKeyboard;
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

            WikiOptions.Add("POE Wiki", "PoeWiki");
            WikiOptions.Add("POE Db", "PoeDb");

            UILanguageOptions = uiLanguageProvider.AvailableLanguages.Select(x => x.Name).ToList();

            nativeKeyboard.OnKeyDown += NativeKeyboard_OnKeyDown;
        }

        public SidekickSettings Settings { get; private set; }

        public Dictionary<string, string> Keybinds { get; private set; } = new Dictionary<string, string>();

        public KeyValuePair<string, string>? CurrentKeybind { get; set; }

        public Dictionary<string, string> WikiOptions { get; private set; } = new Dictionary<string, string>();

        public List<string> UILanguageOptions { get; private set; }

        public void Save()
        {
            var keybindProperties = Settings.GetType().GetProperties();

            Keybinds
                .ToList()
                .ForEach(keybind =>
                {
                    keybindProperties.First(x => x.Name == keybind.Key).SetValue(Settings, keybind.Value);
                });

            AssignValues(Settings, sidekickSettings);
            uiLanguageProvider.SetLanguage(uiLanguageProvider.AvailableLanguages.First(x => x.Name == Settings.UILanguage));
            sidekickSettings.Save();
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Settings)));
        }

        public bool IsKeybindUsed(string keybind, string ignoreKey = null)
        {
            return Keybinds.Any(x => x.Value == keybind && x.Key != ignoreKey);
        }

        private static void AssignValues(SidekickSettings src, SidekickSettings dest)
        {
            // iterates through src Settings (properties) and copies them to dest settings (properties)
            // If there ever comes a time, where some properties do not have to be copied, we can add attributes to exclude them
            src.GetType().GetProperties().ToList().ForEach(x => x.SetValue(dest, x.GetValue(src)));
        }

        private Task NativeKeyboard_OnKeyDown(string key)
        {
            if (key == "Esc")
            {
                CurrentKeybind = null;
                return Task.CompletedTask;
            }

            return Task.CompletedTask;
        }

        public void Dispose()
        {
            nativeKeyboard.OnKeyDown -= NativeKeyboard_OnKeyDown;
        }
    }
}
