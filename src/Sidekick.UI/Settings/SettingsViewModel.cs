using System;
using System.Collections.Generic;
using System.Linq;
using Sidekick.Business.Languages.UI;
using Sidekick.Core.Configuration;

namespace Sidekick.UI.Settings
{
    public class SettingsViewModel : SidekickViewModel<ISettingView>
    {
        private readonly IUILanguageProvider uiLanguageProvider;
        private readonly Configuration configuration;

        public SettingsViewModel(IServiceProvider serviceProvider,
            IUILanguageProvider uiLanguageProvider,
            Configuration configuration)
            : base(serviceProvider)
        {
            this.uiLanguageProvider = uiLanguageProvider;
            this.configuration = configuration;
        }

        public Configuration Configuration { get; private set; }

        public Dictionary<string, string> Keybinds { get; private set; } = new Dictionary<string, string>();

        public new void Open()
        {
            Configuration = new Configuration();
            AssignValues(configuration, Configuration);

            Keybinds.Clear();
            Configuration.GetType()
                .GetProperties()
                .Where(x => x.Name.StartsWith("Key"))
                .ToList()
                .ForEach(x =>
                {
                    Keybinds.Add(x.Name, x.GetValue(Configuration).ToString());
                });

            base.Open();
        }

        public void Save()
        {
            AssignValues(Configuration, configuration);
            uiLanguageProvider.SetLanguage(uiLanguageProvider.AvailableLanguages.First(x => x.Name == Configuration.UILanguage));
            configuration.Save();
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

        private static void AssignValues(Configuration src, Configuration dest)
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
