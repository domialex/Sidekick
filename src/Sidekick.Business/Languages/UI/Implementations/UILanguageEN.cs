namespace Sidekick.Business.Languages.UI.Implementations
{
    [UILanguage("English")]
    public class UILanguageEN : IUILanguage
    {
        public string LanguageName => "English";

        public string TrayIconSettings => "Settings";
        public string TrayIconShowLogs => "Show Logs";
        public string TrayIconExit => "Exit";

        public string SettingsWindowTabGeneral => "General";
        public string SettingsWindowTabKeybindings => "Keybindings";
        public string SettingsWindowWikiSettings => "Wiki Settings";
        public string SettingsWindowWikiDescription => "Choose which Wiki Page should be displayed";
        public string SettingsWindowLanguageSettings => "Language Settings";
        public string SettingsWindowLanguageDescription => "Choose Sidekick's UI Language";

        public string OverlayAccountName => "Account Name";
        public string OverlayCharacter => "Character";
        public string OverlayPrice => "Price";
        public string OverlayItemLevel => "iLvl";
        public string OverlayAge => "Age";

        public string BetrayalAislingTransportaion => "Veiled\nWeapons";
        public string BetrayalAislingFortification => "Veiled\nArmor";
        public string BetrayalAislingResearch => "Add Veiled\nMod(s)";
        public string BetrayalAislingIntervention => "Veiled\nJewellery";
        public string BetrayalCameriaTransportation => "Timeworn\nRelics";
        public string BetrayalCameriaFortification => "Harbinger\nOrbs";
        public string BetrayalCameriaResearch => "Sextants";
        public string BetrayalCameriaIntervention => "Sulphite\nScarab";
        public string BetrayalElreonTransportation => "Unique\nWeapons";
        public string BetrayalElreonFortification => "Unique\nArmor";
        public string BetrayalElreonResearch => "Unique\nJewellery";
        public string BetrayalElreonIntervention => "Reliquary\nScarabs";
        public string BetrayalGraviciusTransportation => "Div Card\nStack";
        public string BetrayalGraviciusFortification => "Random\nDiv Cards";
        public string BetrayalGraviciusResearch => "Swap\nDiv Cards";
        public string BetrayalGraviciusIntervention => "Div Card\nScarab";
        public string BetrayalGuffTransportation => "Chaos\nCrafting";
        public string BetrayalGuffFortification => "Essence\nCrafting";
        public string BetrayalGuffResearch => "Annull\\Ex\nCrafting";
        public string BetrayalGuffIntervention => "Alteration\nCrafting";
        public string BetrayalHakuTransportation => "Rare\nItems";
        public string BetrayalHakuFortification => "Strongboxes";
        public string BetrayalHakuResearch => "Quality\nItems";
        public string BetrayalHakuIntervention => "Strongbox\nScarab";
        public string BetrayalHillockTransportation => "Weapon\nQuality";
        public string BetrayalHillockFortification => "Armor\nQuality";
        public string BetrayalHillockResearch => "Flask\nQuality";
        public string BetrayalHillockIntervention => "Map\nQuality";
        public string BetrayalItThatFledTransportation => "Breach\nSplinter";
        public string BetrayalItThatFledFortification => "Abyss\nJewels";
        public string BetrayalItThatFledResearch => "Upgrade\nBreachstone";
        public string BetrayalItThatFledIntervention => "Breach\nScarab";
        public string BetrayalJanusTransportaion => "Quality\nCurrency";
        public string BetrayalJanusFortification => "Currency\nShards";
        public string BetrayalJanusResearch => "Perandus\nCoins";
        public string BetrayalJanusIntervention => "Perandus\nScarab";
        public string BetrayalJorginTransportation => "Talismans";
        public string BetrayalJorginFortification => "Bestiary\nItems";
        public string BetrayalJorginResearch => "Amulet to\nTalisman";
        public string BetrayalJorginIntervention => "Bestiary\nScarab";
        public string BetrayalKorrellTransportation => "Essences";
        public string BetrayalKorrelFortification => "Map\nFragments";
        public string BetrayalKorrellResearch => "Fossils";
        public string BetrayalKorrelIntervention => "Elder\nScarab";
        public string BetrayalLeoTransportation => "Silver\nCoins";
        public string BetrayalLeoFortification => "Random\nCurrency";
        public string BetrayalLeoResearch => "Currency\nUse";
        public string BetrayalLeoIntervention => "Torment\nScarab";
        public string BetrayalRikerTransportation => "Take One\nCurrency";
        public string BetrayalRikerFortification => "Take One\nUnique";
        public string BetrayalRikerResearch => "Take One\nVeiled";
        public string BetrayalRikerIntervention => "Take On\nDiv Card";
        public string BetrayalRinTransportation => "Normal\nMaps";
        public string BetrayalRinFortification => "Rare\nMaps";
        public string BetrayalRinResearch => "Unique\nMaps";
        public string BetrayalRinIntervention => "Cartography\nScarab";
        public string BetrayalToraTransportation => "Pick Item\n[Timed]";
        public string BetrayalToraFortification => "Enchanted\nItems";
        public string BetrayalToraResearch => "Gem\nExperience";
        public string BetrayalToraIntervention => "Harbinger\nScarab";
        public string BetrayalVaganTransportation => "Rare\nWeapons";
        public string BetrayalVaganFortification => "Rare\nArmor";
        public string BetrayalVaganResearch => "Rare\nJewellery";
        public string BetrayalVaganIntervention => "Rare\nJewels";
        public string BetrayalVoriciTransportation => "Quality\nGems";
        public string BetrayalVoriciFortification => "Linking\nCurrency";
        public string BetrayalVoriciResearch => "White\nSocktes";
        public string BetrayalVoriciIntervention => "Shaper\nScarab";
        public string BetrayalLegendVeryValuable => "Very Valuable";
        public string BetrayalLegendValuable => "Valuable";
        public string BetrayalLegendLessValuable => "Less valuable";
        public string BetrayalLegendNotValuable => "Not valuable";

        public string BetrayalAislingResearchTooltip => "Level1: 1 Mod\nLevel2: Low Chance for 2 Mods\nLevel3: 2 Mods";
        public string BetrayalCameriaTransportationTooltip => "League Specific Items\n(Headhunter possible)";
        public string BetrayalGuffTransportationTooltip => "1 Alch\n200 Chaos\n6 Exalted\n20 Divine\n20 Blessed\n1 Vaal\nLevel1: 8 Seconds\nLevel2: 12 Seconds\nLevel3: 18 Seconds";
        public string BetrayalGuffFortificationTooltip => "20 Scouring\n5 * 4 Essences\n20 Exalted\n1 Vaal\nLevel1: 8 Seconds\nLevel2: 12 Seconds\nLevel3: 18 Seconds";
        public string BetrayalGuffResearchTooltip => "1 Alch\n100 Anullment\n100 Exalted\n1 Divine\n1 Blessed\n1 Vaal\nLevel1: 8 Seconds\nLevel2: 12 Seconds\nLevel3: 18 Seconds";
        public string BetrayalGuffInterventionTooltip => "1 Transmutation\n200 Alteration\n100 AUgmentation\n1 Regal\n1 Exalted\n1 Divine\n1 Vaal\nLevel1: 8 Seconds\nLevel2: 12 Seconds\nLevel3: 18 Seconds";
        public string BetrayalHillockTransportationTooltip => "Level1: 24 %\nLevel2: 26 %\nLevel3: 28%";
        public string BetrayalHillockFortificationTooltip => "Level1: 24 %\nLevel2: 26 %\nLevel3: 28 %";
        public string BetrayalHillockResearchTooltip => "Level1: 22 %\nLevel2: 24 %\nLevel3: 26%";
        public string BetrayalHillockInterventionTooltip => "Level1: 25 %\nLevel2: 30 %\nLevel3: 35 %";
        public string BetrayalItThatFledResearchTooltip => "Level1: Charged\nLevel2: Enriched\nLevel3: Pure";
        public string BetrayalJorginResearchTooltip => "Level1: Level 1 Talisman\nLevel2: Level 2 Talisman\nLevel3: Level 3 Talisman";
        public string BetrayalLeoResearchTooltip => "Level1: Blessed Orb\nLevel2: Divine Orb\nLevel3: Exalted Orb";
        public string BetrayalToraTransportationTooltip => "Time Limit: 8 Seconds\nCurrency\nDivination Cards\nUniques\nItem Level 100 Rare";
        public string BetrayalToraFortificationTooltip => "Level1: Gloves\nLevel2: Boots\nLevel3: Helmet";
        public string BetrayalToraResearchTooltip => "Level1: 20m Exp\nLevel2: 70m Exp\nLevel3: 200m Exp";
        public string BetrayalVoriceResearchTooltip => "Level1: 1 White Socket\nLevel2: 1-2 White Sockets\nLevel3: 1-3 White Sockets";
    }
}
