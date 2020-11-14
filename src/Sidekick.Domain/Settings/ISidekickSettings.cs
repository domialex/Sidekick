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

        bool Overlay_CloseWithMouse { get; }

        string Price_Key_Check { get; }
        bool Price_Prediction_Enable { get; }
        List<string> Price_Mods_Accessory { get; }
        List<string> Price_Mods_Armour { get; }
        List<string> Price_Mods_Flask { get; }
        List<string> Price_Mods_Jewel { get; }
        List<string> Price_Mods_Map { get; }
        List<string> Price_Mods_Weapon { get; }

        string Map_Key_Check { get; }
        string Map_Dangerous_Regex { get; }

        bool EnableCtrlScroll { get; }

        string Key_FindItems { get; }

        string Cheatsheets_Key_Open { get; }
        int Cheatsheets_SelectedIndex { get; }

        string Key_CloseWindow { get; }
        string Key_Exit { get; }
        string Key_GoToHideout { get; }
        string Key_LeaveParty { get; }
        string Key_OpenSearch { get; }
        string Key_OpenSettings { get; }
        string Key_OpenWiki { get; }
        string Key_Stash_Left { get; }
        string Key_Stash_Right { get; }

        bool ShowSplashScreen { get; }

        WikiSetting Wiki_Preferred { get; }
    }
}
