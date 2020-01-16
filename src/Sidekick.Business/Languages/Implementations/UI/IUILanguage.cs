using System;
using System.Collections.Generic;
using System.Text;

namespace Sidekick.Business.Languages.Implementations.UI
{
    public interface IUILanguage
    {
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
