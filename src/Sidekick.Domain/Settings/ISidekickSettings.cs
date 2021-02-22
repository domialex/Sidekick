using System;
using System.Collections.Generic;
using Sidekick.Domain.Wikis;

namespace Sidekick.Domain.Settings
{
    public interface ISidekickSettings
    {
        Guid UserId { get; }
        string Language_Parser { get; }
        string Language_UI { get; }

        string LeagueId { get; }
        string LeaguesHash { get; }

        string Character_Name { get; }
        bool RetainClipboard { get; }
        string Key_FindItems { get; }
        string Key_OpenSettings { get; }
        bool ShowSplashScreen { get; }
        bool SendCrashReports { get; }

        bool Price_CloseWithMouse { get; }
        string Price_Key_Check { get; }
        string Price_Key_OpenSearch { get; }
        string Price_Key_Close { get; }
        bool Price_Prediction_Enable { get; }
        List<string> Price_Mods_Accessory { get; }
        List<string> Price_Mods_Armour { get; }
        List<string> Price_Mods_Flask { get; }
        List<string> Price_Mods_Jewel { get; }
        List<string> Price_Mods_Map { get; }
        List<string> Price_Mods_Weapon { get; }

        bool Map_CloseWithMouse { get; }
        string Map_Key_Close { get; }
        string Map_Key_Check { get; }
        string Map_Dangerous_Regex { get; }

        #region Cheatsheets
        string Cheatsheets_Key_Open { get; }
        string Cheatsheets_Selected { get; }
        string Cheatsheets_Betrayal_Sort { get; }
        #endregion

        List<ChatSetting> Chat_Commands { get; }

        string Stash_Key_Left { get; }
        string Stash_Key_Right { get; }
        bool Stash_EnableCtrlScroll { get; }

        string Wiki_Key_Open { get; }
        WikiSetting Wiki_Preferred { get; }
    }
}
