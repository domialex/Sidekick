namespace Sidekick.Core.Settings
{
    internal static class DefaultSettings
    {
        public static SidekickSettings CreateDefault()
        {
            return new SidekickSettings()
            {
                Language_UI = "en",
                Language_Parser = "English",
                LeagueId = string.Empty,
                Character_Name = string.Empty,
                Wiki_Preferred = WikiSetting.PoeWiki,
                RetainClipboard = true,
                CloseOverlayWithMouse = true,
                EnableCtrlScroll = true,
                Key_CloseWindow = "Space",
                Key_CheckPrices = "Ctrl+D",
                Key_GoToHideout = "F5",
                Key_OpenWiki = "Alt+W",
                Key_FindItems = "Ctrl+F",
                Key_LeaveParty = "F4",
                Key_OpenSearch = "Alt+Q",
                Key_OpenLeagueOverview = "F6",
            };
        }

        public static SidekickSettings Settings = CreateDefault();
    }
}
