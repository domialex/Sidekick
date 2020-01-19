namespace Sidekick.Business.Languages.UI.Implementations
{
    [UILanguage("German")]
    public class UILanguageDE : IUILanguage
    {
        public string LanguageName => "German";

        public string TrayIconSettings => "Einstellungen";
        public string TrayIconShowLogs => "Logs anzeigen";
        public string TrayIconExit => "Beenden";

        public string SettingsWindowTabGeneral => "Allgemein";
        public string SettingsWindowTabKeybindings => "Tastaturkürzel";
        public string SettingsWindowWikiSettings => "Wiki Einstellungen";
        public string SettingsWindowWikiDescription => "Wähle welche Wiki Seite genutzt werden soll";
        public string SettingsWindowLanguageSettings => "Spracheinstellungen";
        public string SettingsWindowLanguageDescription => "Wähle Sidekick's Sprache";

        public string OverlayAccountName => "Acount Name";
        public string OverlayCharacter => "Charakter";
        public string OverlayPrice => "Preis";
        public string OverlayItemLevel => "iLvl";
        public string OverlayAge => "Alter";
    }
}
