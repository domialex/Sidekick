using System.Collections.Generic;

namespace Sidekick.Domain.Settings
{
    public interface ISidekickSettings
    {
        List<string> AccessoryModifiers { get; }
        List<string> ArmourModifiers { get; }
        string Character_Name { get; }
        bool CloseOverlayWithMouse { get; }
        string DangerousModsRegex { get; }
        bool EnableCtrlScroll { get; }
        bool EnablePricePrediction { get; }
        List<string> FlaskModifiers { get; }
        List<string> JewelModifiers { get; }
        string Key_CheckPrices { get; }
        string Key_CloseWindow { get; }
        string Key_Exit { get; }
        string Key_FindItems { get; }
        string Key_GoToHideout { get; }
        string Key_LeaveParty { get; }
        string Key_MapInfo { get; }
        string Key_OpenLeagueOverview { get; }
        string Key_OpenSearch { get; }
        string Key_OpenSettings { get; }
        string Key_OpenWiki { get; }
        string Key_Stash_Left { get; }
        string Key_Stash_Right { get; }
        string Language_Parser { get; }
        string Language_UI { get; }
        int League_SelectedTabIndex { get; }
        string LeagueId { get; }
        string LeaguesHash { get; }
        List<string> MapModifiers { get; }
        bool RetainClipboard { get; }
        bool ShowSplashScreen { get; }
        List<string> WeaponModifiers { get; }
        WikiSetting Wiki_Preferred { get; }
    }
}
