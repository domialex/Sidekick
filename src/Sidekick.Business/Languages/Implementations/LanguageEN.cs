using System;
using System.Text.RegularExpressions;

namespace Sidekick.Business.Languages.Implementations
{
    [Language("English", "Rarity: ")]
    public class LanguageEN : ILanguage
    {
        public Uri PoeTradeSearchBaseUrl => new Uri("https://www.pathofexile.com/trade/search/");
        public Uri PoeTradeExchangeBaseUrl => new Uri("https://www.pathofexile.com/trade/exchange/");
        public Uri PoeTradeApiBaseUrl => new Uri("https://www.pathofexile.com/api/trade/");
        public Uri PoeCdnBaseUrl => new Uri("https://web.poecdn.com/");
        public Uri PoeWebsite => new Uri("https://www.pathofexile.com/");
        public string RarityUnique => "Unique";
        public string RarityRare => "Rare";
        public string RarityMagic => "Magic";
        public string RarityNormal => "Normal";
        public string RarityCurrency => "Currency";
        public string RarityGem => "Gem";
        public string RarityDivinationCard => "Divination Card";
        public string DescriptionUnidentified => "Unidentified";
        public string DescriptionQuality => "Quality: ";
        public string DescriptionCorrupted => "Corrupted";
        public string DescriptionRarity => "Rarity: ";
        public string DescriptionSockets => "Sockets: ";
        public string DescriptionItemLevel => "Item Level: ";
        public string DescriptionExperience => "Experience: ";
        public string DescriptionOrgan => "Uses: ";
        public string DescriptionPhysicalDamage => "Physical Damage: ";
        public string DescriptionElementalDamage => "Elemental Damage: ";
        public string DescriptionEnergyShield => "Energy Shield: ";
        public string DescriptionArmour => "Armour: ";
        public string DescriptionEvasion => "Evasion Rating: ";
        public string DescriptionAttacksPerSecond => "Attacks per Second: ";
        public string DescriptionCriticalStrikeChance => "Critical Strike Chance: ";
        public string PrefixSuperior => "Superior";
        public string InfluenceShaper => "Shaper";
        public string InfluenceElder => "Elder";
        public string InfluenceCrusader => "Crusader";
        public string InfluenceHunter => "Hunter";
        public string InfluenceRedeemer => "Redeemer";
        public string InfluenceWarlord => "Warlord";
        public string DescriptionMapTier => "Map Tier: ";
        public string DescriptionItemQuantity => "Item Quantity: ";
        public string DescriptionItemRarity => "Item Rarity: ";
        public string DescriptionMonsterPackSize => "Monster Pack Size: ";
        public string PrefixBlighted => "Blighted";
        public string KeywordProphecy => "prophecy";
        public string KeywordVaal => "Vaal";
        public string KeywordCatalyst => "Catalyst";
        public string KeywordOil => "Oil";
        public string KeywordIncubator => "Incubator";
        public string KeywordScarab => "Scarab";
        public string KeywordResonator => "Resonator";
        public string KeywordFossil => "Fossil";
        public string KeywordVial => "Vial";
        public string KeywordEssence => "Essence";

        public string PercentagAddedRegexPattern => "^[+]+[\\d{1,2}]+%";
        public string PercentageIncreasedOrDecreasedRegexPattern => "[-+]?[\\d{1,3}]+%";
        public string AttributeIncreasedRegexPattern => "[+]?[\\d{1,3}]+[\\s]";
        public string AttributeRangeRegexPattern => "[\\d{1,3}]+[\\s]+[\\bto\\b]+[\\s]+[\\d{1,3}]+[\\s]";

        public string AttributeCategoryCrafted => "Crafted";
        public string CategoryNameCrafted => "(crafted)";
        public string AttributeCategoryImplicit => "Implicit";
        public string CategoryNameImplicit => "(implicit)";
        public string AttributeCategoryFractured => "Fractured";
        public string CategoryNameFractured => "(fractured)";
        public string AttributeCategoryEnchant => "Enchant";
        public string CategoryNameEnchant => "(enchant)";
        public string AttributeCategoryVeiled => "Veiled";
        public string CategoryNameVeiled => "(veiled)";
        public string AttributeCategoryDelve => "Delve";
        public string CategoryNameDelve => "(delve)";
        public string AttributeCategoryExplicit => "Explicit";
        public string KeywordRange => "to";
    }
}
