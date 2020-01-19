namespace Sidekick.Business.Languages.UI.Implementations
{
    [UILanguage("French")]
    public class UILanguageFR : IUILanguage
    {
        public string LanguageName => "French";

        public string TrayIconSettings => "Configurations";
        public string TrayIconShowLogs => "Afficher les logs";
        public string TrayIconExit => "Quitter";

        public string SettingsWindowTabGeneral => "Général";
        public string SettingsWindowTabKeybindings => "Raccourcis clavier";
        public string SettingsWindowWikiSettings => "Configurations du Wiki";
        public string SettingsWindowWikiDescription => "Sélectionnez quel Wiki sera utilisé";
        public string SettingsWindowLanguageSettings => "Configurations de la langue";
        public string SettingsWindowLanguageDescription => "Sélectionnez la language d'affichage de Sidekick";

        public string OverlayAccountName => "Nom de compte";
        public string OverlayCharacter => "Nom du personnage";
        public string OverlayPrice => "Prix";
        public string OverlayItemLevel => "iLvl";
        public string OverlayAge => "Âge";
    }
}
