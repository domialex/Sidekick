using Microsoft.Extensions.Localization;

namespace Sidekick.Modules.Trade.Localization
{
    public class TradeResources
    {
        private readonly IStringLocalizer<TradeResources> localizer;

        public TradeResources(IStringLocalizer<TradeResources> localizer)
        {
            this.localizer = localizer;
        }

        public string Age_Day => localizer["Age_Day"];
        public string Age_Days => localizer["Age_Days"];
        public string Age_Hour => localizer["Age_Hour"];
        public string Age_Hours => localizer["Age_Hours"];
        public string Age_Minute => localizer["Age_Minute"];
        public string Age_Minutes => localizer["Age_Minutes"];
        public string Age_Now => localizer["Age_Now"];
        public string Age_Seconds => localizer["Age_Seconds"];
        public string Class => localizer["Class"];
        public string Class_Accessory => localizer["Class_Accessory"];
        public string Class_AccessoryAmulet => localizer["Class_AccessoryAmulet"];
        public string Class_AccessoryBelt => localizer["Class_AccessoryBelt"];
        public string Class_AccessoryRing => localizer["Class_AccessoryRing"];
        public string Class_Any => localizer["Class_Any"];
        public string Class_Armour => localizer["Class_Armour"];
        public string Class_ArmourBoots => localizer["Class_ArmourBoots"];
        public string Class_ArmourChest => localizer["Class_ArmourChest"];
        public string Class_ArmourGloves => localizer["Class_ArmourGloves"];
        public string Class_ArmourHelmet => localizer["Class_ArmourHelmet"];
        public string Class_ArmourQuiver => localizer["Class_ArmourQuiver"];
        public string Class_ArmourShield => localizer["Class_ArmourShield"];
        public string Class_Card => localizer["Class_Card"];
        public string Class_Currency => localizer["Class_Currency"];
        public string Class_CurrencyFossil => localizer["Class_CurrencyFossil"];
        public string Class_CurrencyIncubator => localizer["Class_CurrencyIncubator"];
        public string Class_CurrencyPiece => localizer["Class_CurrencyPiece"];
        public string Class_CurrencyResonator => localizer["Class_CurrencyResonator"];
        public string Class_Flask => localizer["Class_Flask"];
        public string Class_Gem => localizer["Class_Gem"];
        public string Class_GemActive => localizer["Class_GemActive"];
        public string Class_GemAwakenedSupport => localizer["Class_GemAwakenedSupport"];
        public string Class_GemSupport => localizer["Class_GemSupport"];
        public string Class_Jewel => localizer["Class_Jewel"];
        public string Class_JewelAbyss => localizer["Class_JewelAbyss"];
        public string Class_JewelBase => localizer["Class_JewelBase"];
        public string Class_JewelCluster => localizer["Class_JewelCluster"];
        public string Class_Leaguestone => localizer["Class_Leaguestone"];
        public string Class_Map => localizer["Class_Map"];
        public string Class_MapFragment => localizer["Class_MapFragment"];
        public string Class_MapScarab => localizer["Class_MapScarab"];
        public string Class_MonsterBeast => localizer["Class_MonsterBeast"];
        public string Class_MonsterSample => localizer["Class_MonsterSample"];
        public string Class_Prophecy => localizer["Class_Prophecy"];
        public string Class_Watchstone => localizer["Class_Watchstone"];
        public string Class_Weapon => localizer["Class_Weapon"];
        public string Class_WeaponBow => localizer["Class_WeaponBow"];
        public string Class_WeaponClaw => localizer["Class_WeaponClaw"];
        public string Class_WeaponDagger => localizer["Class_WeaponDagger"];
        public string Class_WeaponOne => localizer["Class_WeaponOne"];
        public string Class_WeaponOneAxe => localizer["Class_WeaponOneAxe"];
        public string Class_WeaponOneMace => localizer["Class_WeaponOneMace"];
        public string Class_WeaponOneMelee => localizer["Class_WeaponOneMelee"];
        public string Class_WeaponOneSword => localizer["Class_WeaponOneSword"];
        public string Class_WeaponRod => localizer["Class_WeaponRod"];
        public string Class_WeaponRuneDagger => localizer["Class_WeaponRuneDagger"];
        public string Class_WeaponSceptre => localizer["Class_WeaponSceptre"];
        public string Class_WeaponStaff => localizer["Class_WeaponStaff"];
        public string Class_WeaponTwoAxe => localizer["Class_WeaponTwoAxe"];
        public string Class_WeaponTwoMace => localizer["Class_WeaponTwoMace"];
        public string Class_WeaponTwoMelee => localizer["Class_WeaponTwoMelee"];
        public string Class_WeaponTwoSword => localizer["Class_WeaponTwoSword"];
        public string Class_WeaponWand => localizer["Class_WeaponWand"];
        public string Class_WeaponWarstaff => localizer["Class_WeaponWarstaff"];
        public string Corrupted => localizer["Corrupted"];
        public string CountString(int count, int total) => localizer["CountString", count, total];
        public string Error_PoeApi => localizer["Error_PoeApi"];
        public string Filters_Expand => localizer["Filters_Expand"];
        public string Filters_Collapse => localizer["Filters_Collapse"];
        public string Filters_Dps => localizer["Filters_Dps"];
        public string Filters_EDps => localizer["Filters_EDps"];
        public string Filters_Max => localizer["Filters_Max"];
        public string Filters_Min => localizer["Filters_Min"];
        public string Filters_PDps => localizer["Filters_PDps"];
        public string Filters_Armour => localizer["Filters_Armour"];
        public string Filters_Map => localizer["Filters_Map"];
        public string Filters_Misc => localizer["Filters_Misc"];
        public string Filters_Weapon => localizer["Filters_Weapon"];
        public string Filters_Modifiers => localizer["Filters_Modifiers"];
        public string Filters_Pseudo => localizer["Filters_Pseudo"];
        public string Filters_Submit => localizer["Filters_Submit"];
        public string ItemLevel => localizer["ItemLevel"];
        public string Layout => localizer["Layout"];
        public string Layout_Cards_Maximized => localizer["Layout_Cards_Maximized"];
        public string Layout_Cards_Minimized => localizer["Layout_Cards_Minimized"];
        public string LoadMoreData => localizer["LoadMoreData"];
        public string MaxQualityArmour => localizer["MaxQualityArmour"];
        public string MaxQualityDps => localizer["MaxQualityDps"];
        public string MaxQualityEDps => localizer["MaxQualityEDps"];
        public string MaxQualityEnergyShield => localizer["MaxQualityEnergyShield"];
        public string MaxQualityEvasion => localizer["MaxQualityEvasion"];
        public string MaxQualityPDps => localizer["MaxQualityPDps"];
        public string OpenWebsite => localizer["OpenWebsite"];
        public string OverlayAccountName => localizer["OverlayAccountName"];
        public string OverlayAge => localizer["OverlayAge"];
        public string OverlayCharacter => localizer["OverlayCharacter"];
        public string OverlayItemLevel => localizer["OverlayItemLevel"];
        public string OverlayPrice => localizer["OverlayPrice"];
        public string PoeNinja => localizer["PoeNinja"];
        public string PoeNinjaLastUpdated => localizer["PoeNinjaLastUpdated"];
        public string Prediction => localizer["Prediction"];
        public string PredictionConfidence(double confidence) => localizer["PredictionConfidence", confidence.ToString("0.##")];
        public string Requires => localizer["Requires"];
        public string Settings => localizer["Settings"];
        public string Trade => localizer["Trade"];
        public string Unidentified => localizer["Unidentified"];
        public string UpdateNow => localizer["UpdateNow"];
        public string UpdateSeconds => localizer["UpdateSeconds"];
        public string UpdateShortly => localizer["UpdateShortly"];
        public string View_More => localizer["View_More"];
        public string View_Less => localizer["View_Less"];

    }
}
