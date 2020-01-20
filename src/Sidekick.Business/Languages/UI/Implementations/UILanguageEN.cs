namespace Sidekick.Business.Languages.UI.Implementations
{
    [UILanguage("English")]
    public class UILanguageEN : IUILanguage
    {
        public string LanguageName => "English";

        public string TrayIconSettings => "Settings";
        public string TrayIconShowLogs => "Show Logs";
        public string TrayIconExit => "Exit";

        public string SettingsWindowTabGeneral => "General";
        public string SettingsWindowTabKeybindings => "Keybindings";
        public string SettingsWindowWikiSettings => "Wiki Settings";
        public string SettingsWindowWikiDescription => "Choose which Wiki Page should be displayed";
        public string SettingsWindowLanguageSettings => "Language Settings";
        public string SettingsWindowLanguageDescription => "Choose Sidekick's UI Language";

        public string OverlayAccountName => "Account Name";
        public string OverlayCharacter => "Character";
        public string OverlayPrice => "Price";
        public string OverlayItemLevel => "iLvl";
        public string OverlayAge => "Age";
    }
}
