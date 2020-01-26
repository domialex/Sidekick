using System;
using System.Collections.Generic;
using System.Linq;
using Sidekick.Business.Languages.UI;
using Sidekick.Core.Settings;

namespace Sidekick.UI.Settings
{
    public class SettingsViewModel : SidekickViewModel<ISettingView>
    {
        private readonly IUILanguageProvider uiLanguageProvider;
        private readonly SidekickSettings settings;

        public SettingsViewModel(IServiceProvider serviceProvider,
            IUILanguageProvider uiLanguageProvider,
            SidekickSettings settings)
            : base(serviceProvider)
        {
            this.uiLanguageProvider = uiLanguageProvider;
            this.settings = settings;
        }

        public SidekickSettings Settings { get; private set; }

        public Dictionary<string, string> Keybinds { get; private set; } = new Dictionary<string, string>();

        public new void Open()
        {
            Settings = new SidekickSettings();
            AssignValues(settings, Settings);

            Keybinds.Clear();
            Settings.GetType()
                .GetProperties()
                .Where(x => x.Name.StartsWith("Key"))
                .ToList()
                .ForEach(x =>
                {
                    Keybinds.Add(x.Name, x.GetValue(Settings).ToString());
                });

            base.Open();
        }

        public void Save()
        {
            AssignValues(Settings, settings);
            uiLanguageProvider.SetLanguage(uiLanguageProvider.AvailableLanguages.First(x => x.Name == Settings.UILanguage));
            settings.Save();
            Keybinds.Clear();
            Close();
        }

        public void Cancel()
        {
            Keybinds.Clear();
            Close();
        }

        public bool IsKeybindUsed(string keybind, string ignoreKey = null)
        {
            return Keybinds.Any(x => x.Value == keybind && x.Key != ignoreKey);
        }

        private static void AssignValues(SidekickSettings src, SidekickSettings dest)
        {
            dest.CharacterName = src.CharacterName;
            dest.CurrentWikiSettings = src.CurrentWikiSettings;
            dest.KeyCloseWindow = src.KeyCloseWindow;
            dest.KeyFindItems = src.KeyFindItems;
            dest.KeyHideout = src.KeyHideout;
            dest.KeyItemWiki = src.KeyItemWiki;
            dest.KeyLeaveParty = src.KeyLeaveParty;
            dest.KeyOpenLeagueOverview = src.KeyOpenLeagueOverview;
            dest.KeyOpenSearch = src.KeyOpenSearch;
            dest.KeyPriceCheck = src.KeyPriceCheck;
            dest.LeagueId = src.LeagueId;
            dest.RetainClipboard = src.RetainClipboard;
            dest.UILanguage = src.UILanguage;
        }
    }
}
