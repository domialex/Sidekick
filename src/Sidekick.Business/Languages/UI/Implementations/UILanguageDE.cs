namespace Sidekick.Business.Languages.UI.Implementations
{
    [UILanguage("de")]
    public class UILanguageDE : UILanguageEN
    {
        public new string LanguageName => "de";

        public new string TrayIconSettings => "Einstellungen";
        public new string TrayIconShowLogs => "Logs anzeigen";
        public new string TrayIconExit => "Beenden";

        public new string OverlayAccountName => "Acount Name";
        public new string OverlayCharacter => "Charakter";
        public new string OverlayPrice => "Preis";
        public new string OverlayItemLevel => "iLvl";
        public new string OverlayAge => "Alter";
    }
}
