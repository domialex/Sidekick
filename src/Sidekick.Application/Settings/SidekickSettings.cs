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

        public bool Price_CloseWithMouse { get; set; } = false;

        public bool Map_CloseWithMouse { get; set; } = false;

        public bool Stash_EnableCtrlScroll { get; set; } = true;

        public bool Price_Prediction_Enable { get; set; } = true;

        public bool ShowSplashScreen { get; set; } = true;

        public bool SendCrashReports { get; set; } = false;

        public string Map_Dangerous_Regex { get; set; } = "reflect|regen";

        public List<string> Price_Mods_Accessory { get; set; } = new List<string>();
        public List<string> Price_Mods_Armour { get; set; } = new List<string>();
        public List<string> Price_Mods_Flask { get; set; } = new List<string>();
        public List<string> Price_Mods_Jewel { get; set; } = new List<string>();
        public List<string> Price_Mods_Map { get; set; } = new List<string>();
        public List<string> Price_Mods_Weapon { get; set; } = new List<string>();
        public string Price_Layout { get; set; }

        public string Map_Key_Close { get; set; } = "Space";

        public string Price_Key_Close { get; set; } = "Space";

        public string Price_Key_Check { get; set; } = "Ctrl+D";

        public string Map_Key_Check { get; set; } = "Ctrl+X";

        public string Wiki_Key_Open { get; set; } = "Alt+W";

        public string Key_FindItems { get; set; } = "Ctrl+F";

        public string Price_Key_OpenSearch { get; set; } = "Alt+Q";

        public string Key_OpenSettings { get; set; } = "Ctrl+O";

        public string Stash_Key_Left { get; set; } = "";

        public string Stash_Key_Right { get; set; } = "";

        public List<ChatSetting> Chat_Commands { get; set; } = new List<ChatSetting>()
        {
            new ChatSetting("F5", "/hideout", true),
            new ChatSetting("F4", "/kick {Me.CharacterName}", true),
            new ChatSetting("Ctrl+Enter", "@{LastWhisper.CharacterName} ", false),
            new ChatSetting("F12", "/exit", true),
        };

        #region Cheatsheets
        public string Cheatsheets_Key_Open { get; set; } = "F6";
        public string Cheatsheets_Selected { get; set; } = "betrayal";
        public string Cheatsheets_Betrayal_Sort { get; set; } = "default";
        #endregion
    }
}
