using System;
using System.Collections.Generic;
using Sidekick.Domain.Settings;
using Sidekick.Domain.Wikis;

namespace Sidekick.Application.Settings
{
    public class SidekickSettings : ISidekickSettings
    {
        public Guid UserId { get; set; } = Guid.NewGuid();

        public string Language_UI { get; set; } = "en";

        public string Language_Parser { get; set; } = "";

        public string LeagueId { get; set; } = "";

        public string LeaguesHash { get; set; } = "";

        public WikiSetting Wiki_Preferred { get; set; } = WikiSetting.PoeWiki;

        public string Character_Name { get; set; } = "";

        public bool RetainClipboard { get; set; } = true;

        public bool Trade_CloseWithMouse { get; set; } = false;

        public bool Map_CloseWithMouse { get; set; } = false;

        public bool Trade_Prediction_Enable { get; set; } = true;

        public bool ShowSplashScreen { get; set; } = true;

        public bool SendCrashReports { get; set; } = false;

        public string Map_Dangerous_Regex { get; set; } = "reflect|regen";

        public string Trade_Layout { get; set; }

        public string Key_Close { get; set; } = "Space";

        public bool EscapeClosesOverlays { get; set; } = true;

        public string Trade_Key_Check { get; set; } = "Ctrl+D";

        public string Map_Key_Check { get; set; } = "Ctrl+X";

        public string Wiki_Key_Open { get; set; } = "Alt+W";

        public string Key_FindItems { get; set; } = "Ctrl+F";

        public string Trade_Key_OpenSearch { get; set; } = "Alt+Q";

        public string Key_OpenSettings { get; set; } = "F12";

        public string Stash_Key_Left { get; set; } = "";

        public string Stash_Key_Right { get; set; } = "";

        public List<ChatSetting> Chat_Commands { get; set; } = new()
        {
            new ChatSetting("F5", "/hideout", true),
            new ChatSetting("F4", "/kick {Me.CharacterName}", true),
            new ChatSetting("Ctrl+Enter", "@{LastWhisper.CharacterName} ", false),
            new ChatSetting("F9", "/exit", true),
        };

        #region Cheatsheets
        public string Cheatsheets_Key_Open { get; set; } = "F6";
        public string Cheatsheets_Selected { get; set; } = "betrayal";
        public string Cheatsheets_Betrayal_Sort { get; set; } = "default";
        #endregion
    }
}
