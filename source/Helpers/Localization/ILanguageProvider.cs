using System;

namespace Sidekick.Helpers.Localization
{
    public interface ILanguageProvider
    {
        Uri PoeTradeSearchBaseUrl { get; }
        Uri PoeTradeExchangeBaseUrl { get; }
        Uri PoeTradeApiBaseUrl { get; }
        Uri PoeCdnBaseUrl { get; }

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
    }
}
