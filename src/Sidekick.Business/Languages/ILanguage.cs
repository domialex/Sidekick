using System;

namespace Sidekick.Business.Languages
{
    public interface ILanguage
    {
        Uri PoeTradeSearchBaseUrl { get; }
        Uri PoeTradeExchangeBaseUrl { get; }
        Uri PoeTradeApiBaseUrl { get; }
        Uri PoeCdnBaseUrl { get; }
        Uri PoeWebsite { get; }

        string RarityUnique { get; }
        string RarityRare { get; }
        string RarityMagic { get; }
        string RarityNormal { get; }
        string RarityCurrency { get; }
        string RarityGem { get; }
        string RarityDivinationCard { get; }

        string DescriptionUnidentified { get; }
        string DescriptionQuality { get; }
        string DescriptionCorrupted { get; }
        string DescriptionRarity { get; }
        string DescriptionSockets { get; }
        string DescriptionItemLevel { get; }
        string DescriptionMapTier { get; }
        string DescriptionItemQuantity { get; }
        string DescriptionItemRarity { get; }
        string DescriptionMonsterPackSize { get; }
        string DescriptionExperience { get; }
        string DescriptionOrgan { get; }
        string DescriptionPhysicalDamage { get; }
        string DescriptionElementalDamage { get; }
        string DescriptionAttacksPerSecond { get; }
        string DescriptionCriticalStrikeChance { get; }
        string DescriptionEnergyShield { get; }
        string DescriptionArmour { get; }
        string DescriptionEvasion { get; }
        string DescriptionLevel { get; }

        string PrefixSuperior { get; }
        string PrefixBlighted { get; }

        string KeywordProphecy { get; }
        string KeywordVaal { get; }

        string KeywordCatalyst { get; }
        string KeywordOil { get; }
        string KeywordIncubator { get; }
        string KeywordScarab { get; }
        string KeywordResonator { get; }
        string KeywordFossil { get; }
        string KeywordVial { get; }
        string KeywordEssence { get; }

        string InfluenceShaper { get; }
        string InfluenceElder { get; }
        string InfluenceCrusader { get; }
        string InfluenceHunter { get; }
        string InfluenceRedeemer { get; }
        string InfluenceWarlord { get; }


        string AttributeCategoryCrafted { get; }
        string CategoryNameCrafted { get; }
        string AttributeCategoryImplicit { get; }
        string CategoryNameImplicit { get; }
        string AttributeCategoryFractured { get; }
        string CategoryNameFractured { get; }
        string AttributeCategoryEnchant { get; }
        string CategoryNameEnchant { get; }
        string AttributeCategoryVeiled { get; }
        string CategoryNameVeiled { get; }
        string AttributeCategoryDelve { get; }
        string CategoryNameDelve { get; }
        string AttributeCategoryExplicit { get; }
        string PercentageAddedRegexPattern { get; }
        string PercentageIncreasedOrDecreasedRegexPattern { get; }
        string AttributeIncreasedRegexPattern { get; }
        string AttributeRangeRegexPattern { get; }
        string KeywordRange { get; }
    }
}
