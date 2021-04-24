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

        string Key_Close { get; }
        bool EscapeClosesOverlays { get; }
        string LeagueId { get; }
        string LeaguesHash { get; }

        string Character_Name { get; }
        bool RetainClipboard { get; }
        string Key_FindItems { get; }
        string Key_OpenSettings { get; }
        bool ShowSplashScreen { get; }
        bool SendCrashReports { get; }

        bool Trade_CloseWithMouse { get; }
        string Trade_Key_Check { get; }
        string Trade_Key_OpenSearch { get; }
        bool Trade_Prediction_Enable { get; }
        string Trade_Layout { get; }

        bool Map_CloseWithMouse { get; }
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

        string Wiki_Key_Open { get; }
        WikiSetting Wiki_Preferred { get; }
    }
}
