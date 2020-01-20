namespace Sidekick.Business.Languages.UI.Implementations
{
    public interface IUILanguage
    {
        string LanguageName { get; }

        string TrayIconSettings { get; }
        string TrayIconShowLogs { get; }
        string TrayIconExit { get; }

        string SettingsWindowTabGeneral { get; }
        string SettingsWindowTabKeybindings { get; }
        string SettingsWindowWikiSettings { get; }
        string SettingsWindowWikiDescription { get; }
        string SettingsWindowLanguageSettings { get; }
        string SettingsWindowLanguageDescription { get; }

        string OverlayAccountName { get; }
        string OverlayCharacter { get; }
        string OverlayPrice { get; }
        string OverlayItemLevel { get; }
        string OverlayAge { get; }
    }
}
