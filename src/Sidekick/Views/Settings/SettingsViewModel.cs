using System;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using Sidekick.Business.Apis.Poe.Trade.Leagues;
using Sidekick.Business.Caches;
using Sidekick.Core.Initialization;
using Sidekick.Core.Natives;
using Sidekick.Core.Settings;
using Sidekick.Helpers;
using Sidekick.Localization;

namespace Sidekick.Views.Settings
{
    public class SettingsViewModel : IDisposable, INotifyPropertyChanged
    {
        private readonly IUILanguageProvider uiLanguageProvider;
        private readonly SidekickSettings sidekickSettings;
        private readonly INativeKeyboard nativeKeyboard;
        private readonly ILeagueDataService leagueDataService;
        private readonly IKeybindEvents keybindEvents;
        private readonly ICacheService cacheService;
        private readonly IInitializer initializer;
        private bool isDisposed;

        public event PropertyChangedEventHandler PropertyChanged;

        public SettingsViewModel(IUILanguageProvider uiLanguageProvider,
            SidekickSettings sidekickSettings,
            INativeKeyboard nativeKeyboard,
            ILeagueDataService leagueDataService,
            IKeybindEvents keybindEvents,
            ICacheService cacheService,
            IInitializer initializer)
        {
            this.uiLanguageProvider = uiLanguageProvider;
            this.sidekickSettings = sidekickSettings;
            this.nativeKeyboard = nativeKeyboard;
            this.keybindEvents = keybindEvents;
            this.cacheService = cacheService;
            this.initializer = initializer;
            this.leagueDataService = leagueDataService;

            Settings = new SidekickSettings();
            AssignValues(sidekickSettings, Settings);

            Keybinds.Clear();
            Settings.GetType()
                .GetProperties()
                .Where(x => x.Name.StartsWith("Key"))
                .ToList()
                .ForEach(x => Keybinds.Add(x.Name, x.GetValue(Settings).ToString()));

            WikiOptions.Add("POE Wiki", WikiSetting.PoeWiki.ToString());
            WikiOptions.Add("POE Db", WikiSetting.PoeDb.ToString());
            this.leagueDataService.Leagues.ForEach(x => LeagueOptions.Add(x.Id, x.Text));
            uiLanguageProvider.AvailableLanguages.ForEach(x => UILanguageOptions.Add(x.NativeName.First().ToString().ToUpper() + x.NativeName.Substring(1), x.Name));

            nativeKeyboard.OnKeyDown += NativeKeyboard_OnKeyDown;
        }

        public ObservableDictionary<string, string> Keybinds { get; private set; } = new ObservableDictionary<string, string>();

        public ObservableDictionary<string, string> WikiOptions { get; private set; } = new ObservableDictionary<string, string>();

        public ObservableDictionary<string, string> LeagueOptions { get; private set; } = new ObservableDictionary<string, string>();

        public ObservableDictionary<string, string> UILanguageOptions { get; private set; } = new ObservableDictionary<string, string>();

        public string CurrentKey { get; set; }

        public SidekickSettings Settings { get; private set; }

        // This is called when CurrentKey changes, thanks to Fody
        public void OnCurrentKeyChanged()
        {
            keybindEvents.Enabled = string.IsNullOrEmpty(CurrentKey);
        }

        public void Save()
        {
            var keybindProperties = Settings.GetType().GetProperties();

            foreach (var keybind in Keybinds)
            {
                keybindProperties.First(x => x.Name == keybind.Key).SetValue(Settings, keybind.Value);
            };

            var leagueHasChanged = Settings.LeagueId != sidekickSettings.LeagueId;

            AssignValues(Settings, sidekickSettings);
            uiLanguageProvider.SetLanguage(Settings.Language_UI);
            sidekickSettings.Save();

            if (leagueHasChanged) leagueDataService.LeagueChanged();
        }

        public bool IsKeybindUsed(string keybind, string ignoreKey = null)
        {
            return Keybinds.ToCollection().Any(x => x.Value == keybind && x.Key != ignoreKey);
        }

        private void AssignValues(SidekickSettings src, SidekickSettings dest)
        {
            // iterates through src Settings (properties) and copies them to dest settings (properties)
            // If there ever comes a time, where some properties do not have to be copied, we can add attributes to exclude them
            src.GetType().GetProperties().ToList().ForEach(x => x.SetValue(dest, x.GetValue(src)));
        }

        private bool NativeKeyboard_OnKeyDown(string input)
        {
            if (string.IsNullOrEmpty(CurrentKey))
            {
                return false;
            }

            if (input == "Escape")
            {
                CurrentKey = null;
                return true;
            }

            if (!IsKeybindUsed(input, CurrentKey))
            {
                var currentKeybind = Keybinds.ToCollection().FirstOrDefault(x => x.Key == CurrentKey);
                currentKeybind.Value = input;
            }

            CurrentKey = null;
            return true;
        }

        public void Clear(string key)
        {
            Keybinds.ToCollection().FirstOrDefault(x => x.Key == key).Value = string.Empty;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (isDisposed)
            {
                return;
            }

            if (disposing)
            {
                nativeKeyboard.OnKeyDown -= NativeKeyboard_OnKeyDown;
            }

            isDisposed = true;
        }

        public void ResetCache()
        {
            _ = Task.Run(async () =>
            {
                await cacheService.Clear();
                await initializer.Initialize();
            });
        }
    }
}
