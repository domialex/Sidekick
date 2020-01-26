namespace Sidekick.Core.Settings
{
    internal static class DefaultSettings
    {
        public static SidekickSettings CreateDefault()
        {
            return new SidekickSettings()
            {
                UILanguage = "English",
                LeagueId = string.Empty,
                CharacterName = string.Empty,
                CurrentWikiSettings = WikiSetting.PoeWiki,
                RetainClipboard = true,
                CloseOverlayWithMouse = true,
                EnableCtrlScroll = true,
                KeyCloseWindow = "Space",
                KeyPriceCheck = "Ctrl+D",
                KeyHideout = "F5",
                KeyItemWiki = "Alt+W",
                KeyFindItems = "Ctrl+F",
                KeyLeaveParty = "F4",
                KeyOpenSearch = "Alt+Q",
                KeyOpenLeagueOverview = "F6",
            };
        }

        public static SidekickSettings Settings = CreateDefault();
    }
}
