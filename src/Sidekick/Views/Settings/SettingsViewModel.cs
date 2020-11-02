using System;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using MediatR;
using Sidekick.Core.Natives;
using Sidekick.Core.Settings;
using Sidekick.Domain.Cache.Commands;
using Sidekick.Domain.Initialization.Commands;
using Sidekick.Domain.Languages;
using Sidekick.Domain.Languages.Commands;
using Sidekick.Domain.Leagues;
using Sidekick.Extensions;
using Sidekick.Helpers;
using Sidekick.Localization;

namespace Sidekick.Views.Settings
{
    public class SettingsViewModel : IDisposable, INotifyPropertyChanged
    {
#pragma warning disable 67
        public event PropertyChangedEventHandler PropertyChanged;
#pragma warning restore 67

        private readonly IUILanguageProvider uiLanguageProvider;
        private readonly ILanguageProvider languageProvider;
        private readonly SidekickSettings sidekickSettings;
        private readonly INativeKeyboard nativeKeyboard;
        private readonly IKeybindEvents keybindEvents;
        private readonly IMediator mediator;
        private bool isDisposed;

        public SettingsViewModel(
            IUILanguageProvider uiLanguageProvider,
            ILanguageProvider languageProvider,
            SidekickSettings sidekickSettings,
            INativeKeyboard nativeKeyboard,
            IKeybindEvents keybindEvents,
            IMediator mediator)
        {
            this.uiLanguageProvider = uiLanguageProvider;
            this.languageProvider = languageProvider;
            this.sidekickSettings = sidekickSettings;
            this.nativeKeyboard = nativeKeyboard;
            this.keybindEvents = keybindEvents;
            this.mediator = mediator;

            Settings = new SidekickSettings();
            sidekickSettings.CopyValuesTo(Settings);

            Keybinds.Clear();
            Settings.GetType()
                .GetProperties()
                .Where(x => x.Name.StartsWith("Key"))
                .ToList()
                .ForEach(x => Keybinds.Add(x.Name, x.GetValue(Settings).ToString()));

            WikiOptions.Add("POE Wiki", WikiSetting.PoeWiki.ToString());
            WikiOptions.Add("POE Db", WikiSetting.PoeDb.ToString());
            uiLanguageProvider.AvailableLanguages.ForEach(x => UILanguageOptions.Add(x.NativeName.First().ToString().ToUpper() + x.NativeName.Substring(1), x.Name));
            languageProvider.AvailableLanguages.ForEach(x => ParserLanguageOptions.Add(x.Name, x.LanguageCode));

            nativeKeyboard.OnKeyDown += NativeKeyboard_OnKeyDown;
        }

        public async Task Initialize()
        {
            var leagues = await mediator.Send(new GetLeaguesQuery(true));
            leagues.ForEach(x => LeagueOptions.Add(x.Id, x.Text));
        }

        public ObservableDictionary<string, string> Keybinds { get; private set; } = new ObservableDictionary<string, string>();

        public ObservableDictionary<string, string> WikiOptions { get; private set; } = new ObservableDictionary<string, string>();

        public ObservableDictionary<string, string> LeagueOptions { get; private set; } = new ObservableDictionary<string, string>();

        public ObservableDictionary<string, string> UILanguageOptions { get; private set; } = new ObservableDictionary<string, string>();

        public ObservableDictionary<string, string> ParserLanguageOptions { get; private set; } = new ObservableDictionary<string, string>();

        public string CurrentKey { get; set; }

        public SidekickSettings Settings { get; private set; }

        // This is called when CurrentKey changes, thanks to Fody
        public void OnCurrentKeyChanged()
        {
            keybindEvents.Enabled = string.IsNullOrEmpty(CurrentKey);
        }

        public async Task Save()
        {
            var keybindProperties = Settings.GetType().GetProperties();

            foreach (var keybind in Keybinds)
            {
                keybindProperties.First(x => x.Name == keybind.Key).SetValue(Settings, keybind.Value);
            }

            var leagueHasChanged = Settings.LeagueId != sidekickSettings.LeagueId;
            var languageHasChanged = languageProvider.Current.LanguageCode != Settings.Language_Parser;

            Settings.CopyValuesTo(sidekickSettings);
            uiLanguageProvider.SetLanguage(Settings.Language_UI);
            await mediator.Send(new SetLanguageCommand(Settings.Language_Parser));
            sidekickSettings.Save();

            if (languageHasChanged) await ResetCache();
            else if (leagueHasChanged) await mediator.Publish(new LeagueChangedNotification());
        }

        public bool IsKeybindUsed(string keybind, string ignoreKey = null)
        {
            return Keybinds.ToCollection().Any(x => x.Value == keybind && x.Key != ignoreKey);
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

        public async Task ResetCache()
        {
            await mediator.Send(new ClearCacheCommand());
            await mediator.Send(new InitializeCommand(false));
        }
    }
}
