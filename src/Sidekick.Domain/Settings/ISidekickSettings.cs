using System.Collections.Generic;

namespace Sidekick.Domain.Settings
{
    public interface ISidekickSettings
    {
        string Language_Parser { get; }
        string Language_UI { get; }

        string LeagueId { get; }
        string LeaguesHash { get; }

        string Character_Name { get; }
        bool RetainClipboard { get; }

        string Price_Key_Check { get; }
        bool Price_Prediction_Enable { get; }

        bool Overlay_CloseWithMouse { get; }

        List<string> AccessoryModifiers { get; }
        List<string> ArmourModifiers { get; }
        string DangerousModsRegex { get; }
        bool EnableCtrlScroll { get; }
        List<string> FlaskModifiers { get; }
        List<string> JewelModifiers { get; }

        string Key_FindItems { get; }
        string Cheatsheets_Key_Open { get; }
        int Cheatsheets_SelectedIndex { get; }

        string Key_CloseWindow { get; }
        string Key_Exit { get; }
        string Key_GoToHideout { get; }
        string Key_LeaveParty { get; }
        string Key_MapInfo { get; }
        string Key_OpenSearch { get; }
        string Key_OpenSettings { get; }
        string Key_OpenWiki { get; }
        string Key_Stash_Left { get; }
        string Key_Stash_Right { get; }
        List<string> MapModifiers { get; }
        bool ShowSplashScreen { get; }
        List<string> WeaponModifiers { get; }
        WikiSetting Wiki_Preferred { get; }
    }
}
