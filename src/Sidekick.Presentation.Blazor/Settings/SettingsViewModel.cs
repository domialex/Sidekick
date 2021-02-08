using System;
using System.Collections.Generic;
using System.Linq;
using Sidekick.Domain.Settings;
using Sidekick.Domain.Wikis;
using Sidekick.Extensions;

namespace Sidekick.Presentation.Blazor.Settings
{
    public class SettingsViewModel : ISidekickSettings
    {
        public SettingsViewModel(ISidekickSettings sidekickSettings)
        {
            sidekickSettings.CopyValuesTo(this);

            // Make sure to copy by value for the chat commands. Without doing this,
            // changing the chat commands but not saving would still modify the sidekick settings.
            // We do not want that.
            Chat_Commands = sidekickSettings.Chat_Commands
                .Select(x => new ChatSetting(x.Key, x.Command, x.Submit))
                .ToList();

            WikiOptions = new Dictionary<WikiSetting, string>()
            {
                { WikiSetting.PoeWiki, "POE Wiki" },
                { WikiSetting.PoeDb, "POE Db" },
            };
        }

        public Dictionary<WikiSetting, string> WikiOptions { get; private set; }

        public Guid? CurrentKey { get; set; }

        public List<string> Price_Mods_Accessory { get; set; }

        public List<string> Price_Mods_Armour { get; set; }

        public string Character_Name { get; set; }

        public bool Price_CloseWithMouse { get; set; }

        public bool Map_CloseWithMouse { get; set; }

        public string Map_Dangerous_Regex { get; set; }

        public bool Stash_EnableCtrlScroll { get; set; }

        public bool Price_Prediction_Enable { get; set; }

        public List<string> Price_Mods_Flask { get; set; }

        public List<string> Price_Mods_Jewel { get; set; }

        public string Price_Key_Check { get; set; }

        public string Price_Key_Close { get; set; }

        public string Map_Key_Close { get; set; }

        public string Chat_Key_Exit { get; set; }

        public string Key_FindItems { get; set; }

        public string Chat_Key_Hideout { get; set; }

        public string Chat_Key_ReplyToLastWhisper { get; set; }

        public string Chat_Key_LeaveParty { get; set; }

        public string Map_Key_Check { get; set; }

        public string Cheatsheets_Key_Open { get; set; }

        public string Price_Key_OpenSearch { get; set; }

        public string Key_OpenSettings { get; set; }

        public string Wiki_Key_Open { get; set; }

        public string Stash_Key_Left { get; set; }

        public string Stash_Key_Right { get; set; }

        public string Language_Parser { get; set; }

        public string Language_UI { get; set; }

        public string Cheatsheets_Selected { get; set; }

        public string LeagueId { get; set; }

        public string LeaguesHash { get; set; }

        public List<string> Price_Mods_Map { get; set; }

        public bool RetainClipboard { get; set; }

        public bool ShowSplashScreen { get; set; }

        public List<string> Price_Mods_Weapon { get; set; }

        public WikiSetting Wiki_Preferred { get; set; }

        public List<ChatSetting> Chat_Commands { get; set; }
    }
}
