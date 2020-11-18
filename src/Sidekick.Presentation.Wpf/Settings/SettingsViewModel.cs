using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using MediatR;
using Sidekick.Domain.Cache.Commands;
using Sidekick.Domain.Game.Languages;
using Sidekick.Domain.Game.Languages.Commands;
using Sidekick.Domain.Initialization.Commands;
using Sidekick.Domain.Keybinds;
using Sidekick.Domain.Leagues;
using Sidekick.Domain.Settings;
using Sidekick.Domain.Settings.Commands;
using Sidekick.Extensions;
using Sidekick.Presentation.Localization;
using Sidekick.Presentation.Wpf.Helpers;

namespace Sidekick.Presentation.Wpf.Settings
{
    public class SettingsViewModel : IDisposable, INotifyPropertyChanged, ISidekickSettings
    {
#pragma warning disable 67
        public event PropertyChangedEventHandler PropertyChanged;
#pragma warning restore 67

        private readonly IUILanguageProvider uiLanguageProvider;
        private readonly IGameLanguageProvider gameLanguageProvider;
        private readonly ISidekickSettings sidekickSettings;
        private readonly IKeybindsProvider keybindsProvider;
        private readonly IKeybindsExecutor keybindsExecutor;
        private readonly IMediator mediator;
        private bool isDisposed;

        public SettingsViewModel(
            IUILanguageProvider uiLanguageProvider,
            IGameLanguageProvider gameLanguageProvider,
            ISidekickSettings sidekickSettings,
            IKeybindsProvider keybindsProvider,
            IKeybindsExecutor keybindsExecutor,
            IMediator mediator)
        {
            this.uiLanguageProvider = uiLanguageProvider;
            this.gameLanguageProvider = gameLanguageProvider;
            this.sidekickSettings = sidekickSettings;
            this.keybindsProvider = keybindsProvider;
            this.keybindsExecutor = keybindsExecutor;
            this.mediator = mediator;

            sidekickSettings.CopyValuesTo(this);

            WikiOptions.Add("POE Wiki", WikiSetting.PoeWiki.ToString());
            WikiOptions.Add("POE Db", WikiSetting.PoeDb.ToString());
            uiLanguageProvider.AvailableLanguages.ForEach(x => UILanguageOptions.Add(x.NativeName.First().ToString().ToUpper() + x.NativeName[1..], x.Name));
            gameLanguageProvider.AvailableLanguages.ForEach(x => ParserLanguageOptions.Add(x.Name, x.LanguageCode));

            keybindsProvider.OnKeyDown += NativeKeyboard_OnKeyDown;
        }

        public async Task Initialize()
        {
            var leagues = await mediator.Send(new GetLeaguesQuery(true));
            leagues.ForEach(x => LeagueOptions.Add(x.Id, x.Text));
        }

        public ObservableDictionary<string, string> WikiOptions { get; private set; } = new ObservableDictionary<string, string>();

        public ObservableDictionary<string, string> LeagueOptions { get; private set; } = new ObservableDictionary<string, string>();

        public ObservableDictionary<string, string> UILanguageOptions { get; private set; } = new ObservableDictionary<string, string>();

        public ObservableDictionary<string, string> ParserLanguageOptions { get; private set; } = new ObservableDictionary<string, string>();

        public string CurrentKey { get; set; }

        public List<string> Price_Mods_Accessory { get; set; }

        public List<string> Price_Mods_Armour { get; set; }

        public string Character_Name { get; set; }

        public bool Overlay_CloseWithMouse { get; set; }

        public string Map_Dangerous_Regex { get; set; }

        public bool Stash_EnableCtrlScroll { get; set; }

        public bool Price_Prediction_Enable { get; set; }

        public List<string> Price_Mods_Flask { get; set; }

        public List<string> Price_Mods_Jewel { get; set; }

        public string Price_Key_Check { get; set; }

        public string Overlay_Key_Close { get; set; }

        public string Chat_Key_Exit { get; set; }

        public string Key_FindItems { get; set; }

        public string Chat_Key_Hideout { get; set; }

        public string Chat_Key_LeaveParty { get; set; }

        public string Map_Key_Check { get; set; }

        public string Cheatsheets_Key_Open { get; set; }

        public string Price_Key_OpenSearch { get; set; }

        public string Key_OpenSettings { get; set; }

        public string Wiki_Key_Open { get; set; }

        public string Stash_Key_Left { get; set; }

        public string Stash_Key_Right { get; set; }

        public string Language_Parser { get; set; }

        public string Language_UI { get; set; }

        public int Cheatsheets_SelectedIndex { get; set; }

        public string LeagueId { get; set; }

        public string LeaguesHash { get; set; }

        public List<string> Price_Mods_Map { get; set; }

        public bool RetainClipboard { get; set; }

        public bool ShowSplashScreen { get; set; }

        public List<string> Price_Mods_Weapon { get; set; }

        public WikiSetting Wiki_Preferred { get; set; }

        // This is called when CurrentKey changes, thanks to Fody
        public void OnCurrentKeyChanged()
        {
            keybindsExecutor.Enabled = string.IsNullOrEmpty(CurrentKey);
        }

        public async Task Save()
        {
            var leagueHasChanged = LeagueId != sidekickSettings.LeagueId;
            var languageHasChanged = gameLanguageProvider.Current.LanguageCode != Language_Parser;

            uiLanguageProvider.SetLanguage(Language_UI);
            await mediator.Send(new SetGameLanguageCommand(Language_Parser));
            await mediator.Send(new SaveSettingsCommand(this));

            if (languageHasChanged) await ResetCache();
            else if (leagueHasChanged) await mediator.Publish(new LeagueChangedNotification());
        }

        public bool IsKeybindUsed(string keybind, string ignoreKey = null)
        {
            return GetType()
                .GetProperties()
                .Any(x => x.Name != ignoreKey && x.GetValue(this).ToString() == keybind);
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
                var property = GetType()
                    .GetProperties()
                    .FirstOrDefault(x => x.Name == CurrentKey);

                if (property != default)
                {
                    property.SetValue(this, input);
                }
            }

            CurrentKey = null;
            return true;
        }

        public void Clear(string key)
        {
            var property = GetType()
                .GetProperties()
                .FirstOrDefault(x => x.Name == key);
            if (property != default)
            {
                property.SetValue(this, string.Empty);
            }
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
                keybindsProvider.OnKeyDown -= NativeKeyboard_OnKeyDown;
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
