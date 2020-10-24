using System.Collections.Generic;

namespace Sidekick.Core.Settings
{
    public static class DefaultSettings
    {
        public static SidekickSettings CreateDefault()
        {
            return new SidekickSettings()
            {
                HasSetupCompleted = false,
                Language_UI = "en",
                Language_Parser = string.Empty,
                LeagueId = string.Empty,
                League_SelectedTabIndex = 0,
                LeaguesHash = string.Empty,
                Character_Name = string.Empty,
                Wiki_Preferred = WikiSetting.PoeWiki,
                RetainClipboard = true,
                CloseOverlayWithMouse = true,
                EnableCtrlScroll = true,
                EnablePricePrediction = true,
                AccessoryModifiers = new List<string>(),
                ArmourModifiers = new List<string>(),
                FlaskModifiers = new List<string>(),
                JewelModifiers = new List<string>(),
                MapModifiers = new List<string>(),
                WeaponModifiers = new List<string>(),
                DangerousModsRegex = "reflect|regen",
                Key_CloseWindow = "Space",
                Key_CheckPrices = "Ctrl+D",
                Key_MapInfo = "Ctrl+X",
                Key_Exit = "Ctrl+Shift+X",
                Key_FindItems = "Ctrl+F",
                Key_GoToHideout = "F5",
                Key_LeaveParty = "F4",
                Key_OpenSearch = "Alt+Q",
                Key_OpenSettings = "Ctrl+O",
                Key_OpenLeagueOverview = "F6",
                Key_OpenWiki = "Alt+W",
                Key_ReplyToLatestWhisper = "Ctrl+Shift+R",
                Key_Stash_Left = string.Empty,
                Key_Stash_Right = string.Empty,
                ShowSplashScreen = true
            };
        }

        public static SidekickSettings Settings { get; } = CreateDefault();
    }
}
